using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    [SerializeField] private PlayerManager player;
    [SerializeField] private SkillTreeUI skillTreeUI;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();
        skillTreeUI.SetPlayerSkills(player.GetLevelSystem());
    }
}
