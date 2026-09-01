using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(SpriteRenderer))] //[ExecuteAlways] 
public class SpriteRendererAdditional : MonoBehaviour
{
   private SpriteRenderer spriteRenderer;
   private MaterialPropertyBlock propertyBlock;

   private void OnEnable() 
   {
      //    transform.DOLocalRotate(Vector3.up * 180, 0f);
      GetComponent<SpriteRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
      GetComponent<SpriteRenderer>().receiveShadows = true;

      // spriteRenderer = GetComponent<SpriteRenderer>();
      // propertyBlock = new MaterialPropertyBlock();
   }

   void Update()
   {
      // Update the flip properties per instance based on the SpriteRenderer flipX/flipY values
      // propertyBlock.SetFloat("_flipX", spriteRenderer.flipX ? 1 : 0);
      // propertyBlock.SetFloat("_flipY", spriteRenderer.flipY ? 1 : 0);

      // // Apply the property block to this specific SpriteRenderer instance
      // spriteRenderer.SetPropertyBlock(propertyBlock);
   }
}
