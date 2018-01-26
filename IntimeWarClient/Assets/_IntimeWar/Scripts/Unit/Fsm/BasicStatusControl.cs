using HutongGames.PlayMaker;

namespace YJH.Unit
{
    public class BasicStatusControl : FsmStateAbility
    {
        public bool IsDead;
        public FsmInt BodyVisibleControlSeq;
        public bool BodyVisible;
        public bool UIVisible;

        DataMixerBool.Control _bodyVisibleControl;

        public override void OnEnter()
        {
            base.OnEnter();

            if (_unit != null)
            {
                UpdateBodyVisible();


                _unit.STS.IsDead.Value = IsDead;
                _unit.STS.UIVisible.Value = UIVisible;
            }

            Finish();
        }

        void UpdateBodyVisible()
        {
            if (_bodyVisibleControl == null)
            {
                if (BodyVisibleControlSeq.Value < 0)
                {
                    _bodyVisibleControl = _unit.STS.BodyVisible.CreateControl(false);
                    BodyVisibleControlSeq.Value = _bodyVisibleControl.SequencNo;
                }
                else
                {
                    _bodyVisibleControl = _unit.STS.BodyVisible.GetControl(BodyVisibleControlSeq.Value);
                }
            }
            _bodyVisibleControl.Value = BodyVisible;
        }

    }
}