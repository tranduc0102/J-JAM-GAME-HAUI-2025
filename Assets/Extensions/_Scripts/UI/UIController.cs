namespace _Scripts.UI
{
    public class UIController : Singleton<UIController>
    {
        public UISetting UISetting => FindObjectOfType<UISetting>();
        public UIWin UIWin => FindObjectOfType<UIWin>();
        public UIInGame UIInGame => FindObjectOfType<UIInGame>();
        public MessageTutorial Message => FindObjectOfType<MessageTutorial>();
        
        protected override void Awake()
        {
            base.KeepAlive(false);
            base.Awake();
        }
       
    }
}