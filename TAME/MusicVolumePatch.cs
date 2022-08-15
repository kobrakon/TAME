using EFT;
using System.Reflection;
using Aki.Reflection.Patching;

namespace AmbientMusic
{
    public class MusicVolumePatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod() => typeof(GClass908).GetMethod("GetRealSoundValue", BindingFlags.Instance | BindingFlags.Public);

        [PatchPostfix]
        void PostFix(ref GClass908 __instance)
        {
            TAMEController.musicVolume = __instance.MusicVolumeValue;
        }
    }
}
