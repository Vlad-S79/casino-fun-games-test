using UnityEngine.UI;

namespace Core.Ui.Common
{
    public class RaycastElement : Graphic
    {
        public override void SetMaterialDirty() { }
        public override void SetVerticesDirty() { }

        protected override void OnPopulateMesh(VertexHelper vertexHelper) => vertexHelper.Clear();
    }
}