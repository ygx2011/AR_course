/*************************************************************************************

 * 游研堂: www.gamedev3d.com

 *（1）本站致力于为广大的游戏从业者提供相关的资源素材与资讯。

 *（2）本站会持续更新更多相关的资源素材，为游戏领域开发者提供更好的资讯与灵感！

 *（3）本站所有资源素材仅供学习参考，切勿用作商业用途，并请在下载后的24小时内进行删除，

 *     否则由此引发的法律纠纷及连带责任本站和发布者概不承担。
 
*************************************************************************************/
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Attach this to any object that is either a ParticleSystem, or has one or more ParticleSystems
/// as child objects. After all ParticleSystems (including the children and this object's) have finished emitting
/// the object will self-destruct, or alternatively if an audio source is playing will self-destruct after both it
/// and the particle systems have finished playing
/// </summary>
public class ParticleMaster : MonoBehaviour {

    [SerializeField]
    bool waitForAudioSource;

    private List<ParticleSystem> particles;

    void Start()
    {
        particles = new List<ParticleSystem>();

        foreach (Transform child in transform)
        {
            if (child.GetComponent<ParticleSystem>() != null)
            {
                particles.Add(child.GetComponent<ParticleSystem>());
            }
        }

        if (gameObject.GetComponent<ParticleSystem>() != null)
        {
            particles.Add(gameObject.GetComponent<ParticleSystem>());
        }
    }

    public void Emit(bool emit)
    {
        foreach (var particle in particles)
        {
            particle.loop = emit;
        }
    }

    void Update()
    {
        if (waitForAudioSource && GetComponent<AudioSource>() && GetComponent<AudioSource>().isPlaying)
            return;

        foreach (var particle in particles)
        {
            if (particle.IsAlive())
            {
                return;
            }
        }

        Destroy(gameObject);
    }
}
