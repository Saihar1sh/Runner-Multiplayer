using Arixen.ScriptSmith;
using UnityEngine;
using UnityEngine.UI;

public class ViewsManager : MonoGenericLazySingleton<ViewsManager>
{
   [SerializeField] private RenderTexture _renderTextureRef;
   [SerializeField] private RectTransform _viewContainer;
   
   public void OnPlayerJoined(PlayerController player)
   {
      //add a render texture copy
      var renderTexture =  player.CameraHandler.GetCameraView(_renderTextureRef);
      //get output from a new camera attached to new player
      var rawImage = new GameObject(player.IsLocalPlayer? "LocalPlayer" : "RemotePlayer").AddComponent<RawImage>();
      rawImage.transform.SetParent(_viewContainer);
      rawImage.texture = renderTexture;
      player.CameraTextureView = rawImage;
   }
   public void OnPlayerLeft(PlayerController player)
   {
      //delete the copy
      
      //destroy the camera
   }
}
