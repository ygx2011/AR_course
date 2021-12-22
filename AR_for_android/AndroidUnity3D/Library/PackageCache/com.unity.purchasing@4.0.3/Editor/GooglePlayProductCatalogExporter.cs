using System.Text;
using System.Collections.Generic;
using System;
using UnityEngine.Purchasing;
using ExporterValidationResults = UnityEditor.Purchasing.ProductCatalogEditor.ExporterValidationResults;

namespace UnityEditor.Purchasing
{
    /// <summary>
	/// Exports a ProductCatalog to the CSV format expected by Google Play's batch import tools.
	/// </summary>
	internal class GooglePlayProductCatalogExporter : ProductCatalogEditor.IProductCatalogExporter
	{
		public string DisplayName {
			get {
				return "Google Play CSV";
			}
		}

		public string DefaultFileName {
			get {
				return "GooglePlayProductCatalog";
			}
		}

	    public string FileExtension {
			get {
				return "csv";
			}
		}

		public string StoreName {
			get {
				return GooglePlay.Name;
			}
		}

		public string MandatoryExportFolder {
			get {
				return null;
			}
		}

		public List<string> FilesToCopy {
            get {
                return null;
            }
        }

        public bool SaveCompletePackage {
            get {
                return false;
            }
        }

		public string Export(ProductCatalog catalog)
		{
			var fileContents = new StringBuilder();
			var values = new string[8];

			// Write column headers
			values[0] = "Product ID";
			values[1] = "Published State";
			values[2] = "Purchase Type";
			values[3] = "Auto Translate";
			values[4] = "Locale; Title; Description";
			values[5] = "Auto Fill Prices";
			values[6] = "Price";
			values[7] = "Pricing Template ID";
			fileContents.Append(string.Join(kComma, values));
			fileContents.Append("\n");

			foreach (var product in catalog.allProducts) {
				if (string.IsNullOrEmpty(product.GetStoreID(GooglePlay.Name))) {
					values[0] = CSVEscape(product.id);
				} else {
					values[0] = CSVEscape(product.GetStoreID(GooglePlay.Name));
				}

				values[1] = "published";
				values[2] = ProductTypeString(product.type);

				values[3] = kFalse;
				values[4] = PackTitlesAndDescriptions(product);

				if (string.IsNullOrEmpty(product.pricingTemplateID)) {
                    values[5] = kTrue;
					values[6] = PackPrice(product);
					values[7] = string.Empty;
				} else {
                    values[5] = kFalse;
					values[6] = string.Empty;
					values[7] = product.pricingTemplateID;
				}

				fileContents.Append(string.Join(kComma, values));
				fileContents.Append("\n");
			}

			return fileContents.ToString();
		}

		public ExporterValidationResults Validate(ProductCatalog catalog)
		{
			var results = new ExporterValidationResults();

			// Warn if exporting an empty catalog
			if (catalog.allProducts.Count == 0) {
				results.warnings.Add("Catalog is empty");
			}

			// Check for duplicate IDs
			var usedIDs = new HashSet<string>();
			foreach (var product in catalog.allProducts) {
				if (usedIDs.Contains(product.id)) {
					results.errors.Add("More than one product uses the ID \"" + product.id + "\"");
				}
				usedIDs.Add(product.id);
			}

			// Check for duplicate store IDs
			var usedStoreIDs = new HashSet<string>();
			foreach (var product in catalog.allProducts) {
				var storeID = product.GetStoreID(GooglePlay.Name);
				if (!string.IsNullOrEmpty(storeID)) {
					if (usedStoreIDs.Contains(storeID)) {
						results.errors.Add("More than one product uses the Google Play store ID \"" + storeID + "\"");
					}
					usedStoreIDs.Add(product.id);
				}
			}

			// Check for duplicate runtime IDs -- this conflict could occur if a product has a base ID that is the
			// same as another product's store-specific ID
			var runtimeIDs = new HashSet<string>();
			foreach (var product in catalog.allProducts) {
				var runtimeID = product.GetStoreID(GooglePlay.Name);
				if (string.IsNullOrEmpty(runtimeID)) {
					runtimeID = product.id;
				}

				if (runtimeIDs.Contains(runtimeID)) {
					results.errors.Add("More than one product is identified by the ID \"" + runtimeID + "\"");
				}
				runtimeIDs.Add(runtimeID);
			}

			return results;
		}

		public ExporterValidationResults Validate(ProductCatalogItem item)
		{
			var results = new ExporterValidationResults();

			// Check for missing IDs
			if (string.IsNullOrEmpty(item.id)) {
				results.errors.Add("ID is required");
			}

			// A product ID must start with a lowercase letter or a number and must be composed
			// of only lowercase letters (a-z), numbers (0-9), underscores (_), and periods (.)
			string actualID = item.GetStoreID(GooglePlay.Name) ?? item.id;
			string field = (actualID == item.GetStoreID(GooglePlay.Name)) ? "storeID." + GooglePlay.Name : "id";
			if (Char.IsNumber(actualID[0]) || (Char.IsLower(actualID[0]) && Char.IsLetter(actualID[0]))) {
				foreach (char c in actualID) {
					if (c != '_' && c != '.' && !Char.IsNumber(c) && !(Char.IsLetter(c) && Char.IsLower(c))) {
						results.fieldErrors[field] = "Product ID \"" + actualID + "\" must contain only lowercase letters, numbers, underscores, and periods";
					}
				}
			} else {
				results.fieldErrors[field] = "Product ID \"" + actualID + "\" must start with a lowercase letter or a number";
			}

			ValidateDescription(item.defaultDescription, ref results, "defaultDescription");
			foreach (var desc in item.translatedDescriptions) {
				ValidateDescription(desc, ref results);
			}

			// Check for missing price information
			if (string.IsNullOrEmpty(item.pricingTemplateID) && item.googlePrice.value == 0) {
				results.fieldErrors["googlePrice"] = "Items must have either a price or a pricing template ID";
			}

			return results;
		}

		private void ValidateDescription(LocalizedProductDescription desc, ref ExporterValidationResults results, string fieldPrefix = null)
		{
			if (fieldPrefix == null) {
				fieldPrefix = "translatedDescriptions." + desc.googleLocale.ToString();
			}

			// Check for missing title
			if (string.IsNullOrEmpty(desc.Title)) {
				results.fieldErrors[fieldPrefix + ".Title"] = "Title is required (" + desc.googleLocale.ToString() + ")";
			} else {
				if (desc.Title.Length > 55) { // Titles can be up to 55 characters in length
					results.fieldErrors[fieldPrefix + ".Title"] = "Title must not be longer than 55 characters (" + desc.googleLocale.ToString() + ")";
				} else if (desc.Title.Length > 25) { // Titles should be no longer than 25 characters
					results.warnings.Add("Title should not be longer than 25 characters (" + desc.googleLocale.ToString() + ")");
				}
			}

			// Check for missing description
			if (string.IsNullOrEmpty(desc.Description)) {
				results.fieldErrors[fieldPrefix + ".Description"] = "Description is required (" + desc.googleLocale.ToString() + ")";
			} else {
				if (desc.Description.Length > 80) { // Descriptions can be up to 80 characters in length
					results.fieldErrors[fieldPrefix + ".Description"] = "Description must not be longer than 80 characters (" + desc.googleLocale.ToString() + ")";
				}
			}
		}

		private const string kTrue = "true";
		private const string kFalse = "false";
		private const string kComma = ",";
		private const string kSemicolon = ";";
		private const string kBackslash = "\\";
		private const string kQuote = "\"";
		private const string kEscapedQuote = "\"\"";
		private static char[] kCSVCharactersToQuote = { ',', '"', '\n' };

		private static string CSVEscape(string s)
		{
			if (s == null)
				return s;

			if (s.Contains(kQuote)) {
				s = s.Replace(kQuote, kEscapedQuote);
			}

			if (s.IndexOfAny(kCSVCharactersToQuote) > -1) {
				s = kQuote + s + kQuote;
			}

			return s;
		}

		private static string SSVEscape(string s)
		{
			if (s == null)
				return s;

			s.Replace(kBackslash, kBackslash + kBackslash);
			s.Replace(kSemicolon, kBackslash + kSemicolon);
			return s;
		}

		private static string ProductTypeString(ProductType type)
		{
			return "managed_by_android";
		}

		private static string PackTitlesAndDescriptions(ProductCatalogItem product)
		{
			var values = new List<string>();

			values.Add(product.defaultDescription.googleLocale.ToString());
			values.Add(SSVEscape(product.defaultDescription.Title));
			values.Add(SSVEscape(product.defaultDescription.Description));

			foreach (var desc in product.translatedDescriptions) {
				values.Add(desc.googleLocale.ToString());
				values.Add(SSVEscape(desc.Title));
				values.Add(SSVEscape(desc.Description));
			}

			return CSVEscape(string.Join(kSemicolon, values.ToArray()));
		}

		private const int kPriceMicroUnitMultiplier = 1000000;
		private static string PackPrice(ProductCatalogItem product)
		{
			return CSVEscape(Convert.ToInt32(product.googlePrice.value * kPriceMicroUnitMultiplier).ToString());
		}

		public ProductCatalog NormalizeToType(ProductCatalog catalog)
		{
			return catalog;
		}
	}

}
