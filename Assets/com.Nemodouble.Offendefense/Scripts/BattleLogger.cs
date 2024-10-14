using com.Nemodouble.Offendefense.Scripts.Skills;
using com.Nemodouble.Offendefense.Scripts.UIs;
using UnityEngine;

namespace com.Nemodouble.Offendefense.Scripts
{
    public class BattleLogger : MonoBehaviour
    {
        // singleton
        private static BattleLogger _instance;
        
        public static BattleLogger Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<BattleLogger>();
                }
                return _instance;
            }
        }
        public GameObject BattleLogPrefab;
        private BattleLogUi _nowBattleLogger;

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
            }
            else if (_instance != this)
            {
                Destroy(gameObject);
            }

            _nowBattleLogger = Instantiate(BattleLogPrefab, transform.parent).GetComponent<BattleLogUi>();
            _nowBattleLogger.gameObject.SetActive(false);
        }
        
        public void AddLog(SkillResult mySkillResult, SkillResult oppSkillResult)
        {
            _nowBattleLogger.GetComponent<BattleLogUi>().SetText(mySkillResult, oppSkillResult);
            _nowBattleLogger.gameObject.SetActive(true);
            _nowBattleLogger.transform.SetParent(transform);
            _nowBattleLogger.transform.SetSiblingIndex(0);
            _nowBattleLogger = Instantiate(BattleLogPrefab, transform.parent).GetComponent<BattleLogUi>();
        }
        
        public void UpdateMySkill(SkillType skillType)
        {
            _nowBattleLogger.myImage.sprite = SkillFactory.GetSkillBySkillType(skillType).Icon;
        }

        public void UpdateOppSkill(SkillType skillType)
        {
            _nowBattleLogger.oppImage.sprite = SkillFactory.GetSkillBySkillType(skillType).Icon;
        }
    }
}