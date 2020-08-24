using NexHUD.Audio;
using NexHUD.Inputs;
using NexHUD.Ui.Common;
using NexHUDCore;
using NexHUDCore.NxItems;

namespace NexHUD.Ui
{
    public class UiMainRadio : NxGroup
    {
        public const int RadioWidth = 300;
        public const int RadioNextPrevWidth = 60;
        public const int Height = 50;
        public NxMenu Menu;
        public NxButton PreviousRadio;
        public NxButton NextRadio;
        public NxButton Radio;

        public NxProgressBar Volume;

        public bool VolumeSelected;

        public int totalWidth { get { return RadioWidth + RadioNextPrevWidth * 2 + 4; } }

        public UiMainRadio(int _x, int _y, NxMenu _menu) : base(_menu.frame.NxOverlay)
        {
            Menu = _menu;
            x = _x;
            y = _y;
            PreviousRadio = new NxButton(x - RadioWidth / 2 - RadioNextPrevWidth - 2, y, RadioNextPrevWidth, Height, "<<", Menu);
            NextRadio = new NxButton(x + RadioWidth / 2 + 2, y, RadioNextPrevWidth, Height, ">>", Menu);
            Radio = new NxButton(x - RadioWidth / 2, y, RadioWidth, Height, "[ ] Radio Sidewinder", Menu);

            Add(PreviousRadio);
            Add(NextRadio);
            Add(Radio);

            Volume = new NxProgressBar(x - RadioWidth / 2 - RadioNextPrevWidth - 2, y + Height + 10, totalWidth, 10, EDColors.YELLOW);
            Volume.Value = (int)(RadioPlayer.Instance.Volume * 100);
            Add(Volume);
        }

        public override void Update()
        {
            base.Update();

            bool select = NexHudEngine.isShortcutPressed(Shortcuts.get(ShortcutId.select));
            bool left = NexHudEngine.isShortcutPressed(Shortcuts.get(ShortcutId.left));
            bool right = NexHudEngine.isShortcutPressed(Shortcuts.get(ShortcutId.right));

            if (select)
            {
                if (Radio.Selected)
                {
                    if (RadioPlayer.Instance.isPlaying)
                        RadioPlayer.Instance.Pause();
                    else
                        RadioPlayer.Instance.Play();
                }
                else if (PreviousRadio.Selected)
                    RadioPlayer.Instance.Prev();
                else if (NextRadio.Selected)
                    RadioPlayer.Instance.Next();
            }

            Radio.Label = string.Format("{0} {1}", RadioPlayer.Instance.isPlaying ? "[>]" : "[ ]", RadioPlayer.Instance.getRadioInfos().name);
           // Volume.width = (int)(RadioPlayer.Instance.Volume * totalWidth);

            if( VolumeSelected)
            {
                if (right)
                    RadioPlayer.Instance.VolumeUp();
                else if (left)
                    RadioPlayer.Instance.VolumeDown();
            }
            Volume.Color = VolumeSelected ? EDColors.YELLOW : EDColors.getColor(EDColors.ORANGE, 0.6f);
            Volume.Value = (int)(RadioPlayer.Instance.Volume * 100.0f);
        }
    }
}
