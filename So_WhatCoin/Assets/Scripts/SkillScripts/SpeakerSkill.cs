using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeakerSkill : SkillCoolTime
{
    [SerializeField]
    GameObject speakerOn;
    [SerializeField]
    ParticleSystem CoinRain;

    public override void Skill()
    {
        StartCoroutine(SpeakerSkillOn());
    }

    IEnumerator SpeakerSkillOn()
    {
        uint playerAutomatcIncome = GameManager.Instance.player.playerData.automatcIncome;

        laptopClick.isSkill = true;
        speakerOn.SetActive(true);
        SoundManager.instance.BgSoundPlay(2);
        GameManager.Instance.player.playerData.automatcIncome += playerAutomatcIncome * 3;

        yield return new WaitForSeconds(1f);

        CoinRain.Play();

        yield return new WaitForSeconds(5f);
        GameManager.Instance.player.playerData.automatcIncome -= playerAutomatcIncome * 3;

        SoundManager.instance.BgSoundPlay(0);
        CoinRain.Stop();
        speakerOn.SetActive(false);
        laptopClick.isSkill = false;
    }
}
