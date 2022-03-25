/*************************************************************************************

 * 游研堂: www.gamedev3d.com

 *（1）本站致力于为广大的游戏从业者提供相关的资源素材与资讯。

 *（2）本站会持续更新更多相关的资源素材，为游戏领域开发者提供更好的资讯与灵感！

 *（3）本站所有资源素材仅供学习参考，切勿用作商业用途，并请在下载后的24小时内进行删除，

 *     否则由此引发的法律纠纷及连带责任本站和发布者概不承担。
 
*************************************************************************************/
using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(iTweenPath))]
public class iTweenPathEditor : Editor
{
	iTweenPath _target;
	GUIStyle style = new GUIStyle();
	public static int count = 0;
	
	void OnEnable(){
		//i like bold handle labels since I'm getting old:
		style.fontStyle = FontStyle.Bold;
		style.normal.textColor = Color.white;
		_target = (iTweenPath)target;
		
		//lock in a default path name:
		if(!_target.initialized){
			_target.initialized = true;
			_target.pathName = "New Path " + ++count;
			_target.initialName = _target.pathName;
		}
	}
	
	public override void OnInspectorGUI(){		
		//draw the path?
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.PrefixLabel("Path Visible");
		_target.pathVisible = EditorGUILayout.Toggle(_target.pathVisible);
		EditorGUILayout.EndHorizontal();
		
		//path name:
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.PrefixLabel("Path Name");
		_target.pathName = EditorGUILayout.TextField(_target.pathName);
		EditorGUILayout.EndHorizontal();
		
		if(_target.pathName == ""){
			_target.pathName = _target.initialName;
		}
		
		//path color:
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.PrefixLabel("Path Color");
		_target.pathColor = EditorGUILayout.ColorField(_target.pathColor);
		EditorGUILayout.EndHorizontal();
		
		//exploration segment count control:
		EditorGUILayout.BeginHorizontal();
		//EditorGUILayout.PrefixLabel("Node Count");
		_target.nodeCount = Mathf.Max(2, EditorGUILayout.IntField("Node Count", _target.nodeCount));
		//_target.nodeCount =  Mathf.Clamp(EditorGUILayout.IntSlider(_target.nodeCount, 0, 10), 2,100);
		EditorGUILayout.EndHorizontal();
		
		//add node?
		if(_target.nodeCount > _target.nodes.Count){
			for (int i = 0; i < _target.nodeCount - _target.nodes.Count; i++) {
				_target.nodes.Add(Vector3.zero);	
			}
		}
	
		//remove node?
		if(_target.nodeCount < _target.nodes.Count){
			if(EditorUtility.DisplayDialog("Remove path node?","Shortening the node list will permantently destory parts of your path. This operation cannot be undone.", "OK", "Cancel")){
				int removeCount = _target.nodes.Count - _target.nodeCount;
				_target.nodes.RemoveRange(_target.nodes.Count-removeCount,removeCount);
			}else{
				_target.nodeCount = _target.nodes.Count;	
			}
		}
				
		//node display:
		EditorGUI.indentLevel = 4;
		for (int i = 0; i < _target.nodes.Count; i++) {
			_target.nodes[i] = EditorGUILayout.Vector3Field("Node " + (i+1), _target.nodes[i]);
		}
		
		//update and redraw:
		if(GUI.changed){
			EditorUtility.SetDirty(_target);			
		}
	}
	
	void OnSceneGUI(){
		if(_target.pathVisible){			
			if(_target.nodes.Count > 0){
				//allow path adjustment undo:
				Undo.SetSnapshotTarget(_target,"Adjust iTween Path");
				
				//path begin and end labels:
				Handles.Label(_target.nodes[0], "'" + _target.pathName + "' Begin", style);
				Handles.Label(_target.nodes[_target.nodes.Count-1], "'" + _target.pathName + "' End", style);
				
				//node handle display:
				for (int i = 0; i < _target.nodes.Count; i++) {
					_target.nodes[i] = Handles.PositionHandle(_target.nodes[i], Quaternion.identity);
				}	
			}	
		}
	}
}