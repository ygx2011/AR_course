/*************************************************************************************

 * 游研堂: www.gamedev3d.com

 *（1）本站致力于为广大的游戏从业者提供相关的资源素材与资讯。

 *（2）本站会持续更新更多相关的资源素材，为游戏领域开发者提供更好的资讯与灵感！

 *（3）本站所有资源素材仅供学习参考，切勿用作商业用途，并请在下载后的24小时内进行删除，

 *     否则由此引发的法律纠纷及连带责任本站和发布者概不承担。
 
*************************************************************************************/
using UnityEngine;
using System.Collections;

public class MarioSound : MonoBehaviour {

    public AudioSource SoundSource;
    public AudioSource VoiceSource;

    public AudioClip Footstep;
    public AudioClip Skid;
    public AudioClip Land;
    public AudioClip GroundPound;
    public AudioClip Teleport;
    public AudioClip GetLife;
    public AudioClip Impact;
    public AudioClip Upgrade;
    public AudioClip Spring;
    public AudioClip Spin;
    public AudioClip FlipIntoLevel;
    public AudioClip Slide;
    public AudioClip DiveLand;

    public AudioClip[] SingleJump;
    public AudioClip DoubleJump;
    public AudioClip[] TripleJump;
    public AudioClip[] SideFlip;
    public AudioClip[] BackFlip;
    public AudioClip LongJump;
    public AudioClip JumpKick;
    public AudioClip WallHit;
    public AudioClip Climb;
    public AudioClip Hang;
    public AudioClip TakeDamage;
    public AudioClip PunchSingle;
    public AudioClip PunchDouble;
    public AudioClip Dive;
    public AudioClip Die;
    public AudioClip GroundPoundDown;
    public AudioClip[] SlideSpin;
    public AudioClip HeavyKnockback;
    public AudioClip LandHurt;

    private IEnumerator voiceRoutine;

    void PlayVoice(AudioClip clip)
    {
        if (voiceRoutine != null)
            StopCoroutine(voiceRoutine);

        VoiceSource.Stop();

        VoiceSource.PlayOneShot(clip);
    }

    void PlayRandomVoice(AudioClip[] clips)
    {
        if (voiceRoutine != null)
            StopCoroutine(voiceRoutine);

        VoiceSource.Stop();

        VoiceSource.PlayOneShot(clips[Random.Range(0, clips.Length)]);
    }

    void PlayRandomVoice(AudioClip[] clips, float delay)
    {
        if (voiceRoutine != null)
            StopCoroutine(voiceRoutine);

        VoiceSource.Stop();

        voiceRoutine = PlayClipWithDelay(VoiceSource, clips[Random.Range(0, clips.Length)], delay);
        StartCoroutine(voiceRoutine);
    }

    IEnumerator PlayClipWithDelay(AudioSource source, AudioClip clip, float delay)
    {
        yield return new WaitForSeconds(delay);

        source.PlayOneShot(clip);
    }

    public void StopVoices()
    {
        if (voiceRoutine != null)
            StopCoroutine(voiceRoutine);
    }

    public void EndFootsteps()
    {
        StopAllCoroutines();
    }

    public void PlaySingleJump()
    {
        PlayRandomVoice(SingleJump, 0f);
    }

    public void PlayDoubleJump()
    {
        PlayVoice(DoubleJump);
    }

    public void PlayTripleJump()
    {
        PlayRandomVoice(TripleJump);
    }

    public void PlayLongJump()
    {
        PlayVoice(LongJump);
    }

    public void PlaySideFlip()
    {
        PlayRandomVoice(SideFlip);
    }

    public void PlayBackFlip()
    {
        PlayRandomVoice(BackFlip);
    }

    public void PlaySlideSpin()
    {
        PlayRandomVoice(SlideSpin);
    }

    public void PlayJumpKick()
    {
        PlayVoice(JumpKick);
    }

    public void PlayClimb()
    {
        PlayVoice(Climb);
    }

    public void PlayHang()
    {
        PlayVoice(Hang);
    }

    public void PlayTakeDamage()
    {
        PlayVoice(TakeDamage);
    }

    public void PlayWallHit()
    {
        PlayVoice(WallHit);
    }

    public void PlayWallKick()
    {
        PlayRandomVoice(SingleJump);
    }

    public void PlaySinglePunch()
    {
        PlayVoice(PunchSingle);
    }

    public void PlayDoublePunch()
    {
        PlayVoice(PunchDouble);
    }

    public void PlayKick()
    {
        PlayVoice(JumpKick);
    }

    public void PlayDive()
    {
        PlayVoice(Dive);
    }

    public void PlayGroundDive()
    {
        PlayRandomVoice(SingleJump);
    }

    public void PlayLandHurt()
    {
        PlayVoice(LandHurt);
    }

    public void PlayDie()
    {
        PlayVoice(Die);
    }

    public void PlayGroundPoundDown()
    {
        PlayVoice(GroundPoundDown);
    }

    public void PlayHeavyKnockback()
    {
        PlayVoice(HeavyKnockback);
    }

    public void PlayTeleport()
    {
        SoundSource.PlayOneShot(Teleport);
    }

    public void PlayFlipIntoLevel()
    {
        SoundSource.PlayOneShot(FlipIntoLevel);
    }

    public void PlaySkid()
    {
        SoundSource.PlayOneShot(Skid, 0.6f);
    }

    public void PlayImpact()
    {
        SoundSource.PlayOneShot(Impact, 0.6f);
    }

    public void PlayLand()
    {
        SoundSource.PlayOneShot(Land, 0.5f);
    }

    public void PlayGroundPound()
    {
        SoundSource.PlayOneShot(GroundPound);
    }

    public void PlaySpin()
    {
        SoundSource.PlayOneShot(Spin);
    }

    public void PlayUpgrade()
    {
        SoundSource.PlayOneShot(Upgrade);
    }

    public void PlayGetLife()
    {
        SoundSource.PlayOneShot(GetLife);
    }

    public void PlaySpring()
    {
        SoundSource.PlayOneShot(Spring);
    }

    public void PlayDiveLand()
    {
        SoundSource.PlayOneShot(DiveLand);
    }

    public void PlaySlide()
    {
        SoundSource.loop = true;
        SoundSource.clip = Slide;
        SoundSource.volume = 0.75f;
        SoundSource.Play();
    }

    public void Stop()
    {
        SoundSource.Stop();
        SoundSource.volume = 1;
        SoundSource.loop = false;
    }

    public void StartFootsteps(float speed, float delay)
    {
        StopAllCoroutines();
        StartCoroutine(Footsteps(speed, delay));
    }

    IEnumerator Footsteps(float speed, float delay)
    {
        float lastStepTime = Time.time - speed + delay;

        while (true)
        {
            if (SuperMath.Timer(lastStepTime, speed))
            {
                SoundSource.PlayOneShot(Footstep, 0.07f);
                lastStepTime = Time.time;
            }

            yield return 0;
        }
    }
}
