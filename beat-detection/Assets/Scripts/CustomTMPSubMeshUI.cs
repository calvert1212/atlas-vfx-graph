using UnityEngine;
using TMPro;

public class CustomTMPSubMeshUI : TMP_SubMeshUI
{
    protected override void UpdateMaterial()
    {
        //Debug.Log("*** STO-UI - UpdateMaterial() *** FRAME (" + Time.frameCount + ")");

        if (sharedMaterial == null)
            return;

        // Special handling to keep the Culling of the material in sync with parent text object
        if (textComponent.fontSharedMaterial != null && sharedMaterial.HasProperty(ShaderUtilities.ShaderTag_CullMode))
        {
            float cullMode = textComponent.fontSharedMaterial.GetFloat(ShaderUtilities.ShaderTag_CullMode);
            sharedMaterial.SetFloat(ShaderUtilities.ShaderTag_CullMode, cullMode);
        }

        canvasRenderer.materialCount = 1;
        canvasRenderer.SetMaterial(materialForRendering, 0);

        #if UNITY_EDITOR
        if (sharedMaterial != null && gameObject.name != "TMP SubMeshUI [" + sharedMaterial.name + "]")
            gameObject.name = "TMP SubMeshUI [" + sharedMaterial.name + "]";
        #endif
    }
}