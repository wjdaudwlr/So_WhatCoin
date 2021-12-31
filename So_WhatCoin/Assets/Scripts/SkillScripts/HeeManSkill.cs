using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeeManSkill : SkillCoolTime
{
    [SerializeField]
    private GameObject heeManBackground;
    [SerializeField]
    private GameObject heeManImgae;

    public override void Skill()
    {
        StartCoroutine(HeeManSkillOn());
    }

    IEnumerator HeeManSkillOn()
    {
        SoundManager.instance.BgSoundPlay(1);

        laptopClick.isHeeManSkill = true;
        laptopClick.isSkill = true;
        heeManBackground.SetActive(true);
        heeManImgae.SetActive(true);

        yield return new WaitForSeconds(6f);

        SoundManager.instance.BgSoundPlay(0);

        laptopClick.isHeeManSkill = false;
        laptopClick.isSkill = false;
        heeManBackground.SetActive(false);
        heeManImgae.SetActive(false);
    }
}
