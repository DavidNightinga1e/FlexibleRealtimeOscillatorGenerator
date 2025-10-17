using UnityEngine;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime
{

[RequireComponent(typeof(CanvasRenderer))]
public class UILineRenderer : MaskableGraphic
{
    [SerializeField] private Texture m_Texture;
    [SerializeField] private Rect m_UVRect = new Rect(0f, 0f, 1f, 1f);
    
    [Header("Line Settings")]
    [SerializeField] private Vector2[] m_Points;
    [SerializeField] private float m_Thickness = 5f;
    [SerializeField] private bool m_UseMargins;
    [SerializeField] private Vector2 m_Margin;
    [SerializeField] private bool m_RelativeSize;
    
    [Header("Line Style")]
    [SerializeField] private bool m_ClosedLoop = false;
    [SerializeField] private float m_LineCapSize = 10f;
    [SerializeField] private bool m_AddStartCap = false;
    [SerializeField] private bool m_AddEndCap = false;

    public Vector2[] Points
    {
        get => m_Points;
        set
        {
            m_Points = value;
            SetVerticesDirty();
        }
    }

    public float Thickness
    {
        get => m_Thickness;
        set
        {
            m_Thickness = value;
            SetVerticesDirty();
        }
    }

    public override Texture mainTexture => m_Texture != null ? m_Texture : base.mainTexture;

    protected UIVertex[] SetVbo(Vector2[] vertices, Vector2[] uvs)
    {
        UIVertex[] vbo = new UIVertex[4];
        for (int i = 0; i < vertices.Length; i++)
        {
            var vert = UIVertex.simpleVert;
            vert.color = color;
            vert.position = vertices[i];
            vert.uv0 = uvs[i];
            vbo[i] = vert;
        }
        return vbo;
    }

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        if (m_Points == null || m_Points.Length < 2)
        {
            vh.Clear();
            return;
        }

        vh.Clear();

        var pointList = new Vector2[m_Points.Length];
        for (int i = 0; i < m_Points.Length; i++)
        {
            pointList[i] = m_Points[i];
        }

        if (m_UseMargins)
        {
            if (m_RelativeSize)
            {
                pointList[0] = new Vector2(m_Margin.x, m_Margin.y);
                pointList[pointList.Length - 1] = new Vector2(rectTransform.rect.width - m_Margin.x, rectTransform.rect.height - m_Margin.y);
            }
            else
            {
                var size = rectTransform.rect.size;
                pointList[0] = new Vector2(m_Margin.x, m_Margin.y);
                pointList[pointList.Length - 1] = new Vector2(size.x - m_Margin.x, size.y - m_Margin.y);
            }
        }

        DrawLine(pointList, vh);
    }

    private void DrawLine(Vector2[] points, VertexHelper vh)
    {
        if (points.Length < 2) return;

        // Draw line segments
        for (int i = 0; i < points.Length - 1; i++)
        {
            DrawLineSegment(points[i], points[i + 1], vh, i == 0 && m_AddStartCap, i == points.Length - 2 && m_AddEndCap);
        }

        // Close the loop if needed
        if (m_ClosedLoop && points.Length > 2)
        {
            DrawLineSegment(points[points.Length - 1], points[0], vh, false, false);
        }
    }

    private void DrawLineSegment(Vector2 point1, Vector2 point2, VertexHelper vh, bool addStartCap, bool addEndCap)
    {
        Vector2 direction = (point2 - point1).normalized;
        Vector2 perpendicular = new Vector2(-direction.y, direction.x) * (m_Thickness * 0.5f);

        // Calculate the four vertices for the line segment
        Vector2[] vertices = new Vector2[4];
        vertices[0] = point1 - perpendicular;
        vertices[1] = point1 + perpendicular;
        vertices[2] = point2 + perpendicular;
        vertices[3] = point2 - perpendicular;

        // Add start cap
        if (addStartCap)
        {
            AddLineCap(point1, -direction, vh, true);
        }

        // Add the quad for the line segment
        AddQuad(vertices, vh);

        // Add end cap
        if (addEndCap)
        {
            AddLineCap(point2, direction, vh, false);
        }
    }

    private void AddQuad(Vector2[] vertices, VertexHelper vh)
    {
        if (vertices.Length != 4) return;

        int currentVertCount = vh.currentVertCount;
        
        // Add vertices
        for (int i = 0; i < 4; i++)
        {
            var vert = UIVertex.simpleVert;
            vert.position = vertices[i];
            vert.uv0 = GetUVForPosition(i);
            vert.color = color;
            vh.AddVert(vert);
        }

        // Add triangles
        vh.AddTriangle(currentVertCount, currentVertCount + 1, currentVertCount + 2);
        vh.AddTriangle(currentVertCount + 2, currentVertCount + 3, currentVertCount);
    }

    private void AddLineCap(Vector2 position, Vector2 direction, VertexHelper vh, bool isStart)
    {
        Vector2 perpendicular = new Vector2(-direction.y, direction.x) * (m_Thickness * 0.5f);
        Vector2 capDirection = isStart ? -direction : direction;
        
        Vector2[] vertices = new Vector2[3];
        vertices[0] = position - perpendicular;
        vertices[1] = position + perpendicular;
        vertices[2] = position + capDirection * m_LineCapSize;

        int currentVertCount = vh.currentVertCount;
        
        for (int i = 0; i < 3; i++)
        {
            var vert = UIVertex.simpleVert;
            vert.position = vertices[i];
            vert.uv0 = GetUVForPosition(i);
            vert.color = color;
            vh.AddVert(vert);
        }

        vh.AddTriangle(currentVertCount, currentVertCount + 1, currentVertCount + 2);
    }

    private Vector2 GetUVForPosition(int vertexIndex)
    {
        switch (vertexIndex)
        {
            case 0: return new Vector2(0f, 0f);
            case 1: return new Vector2(0f, 1f);
            case 2: return new Vector2(1f, 1f);
            case 3: return new Vector2(1f, 0f);
            default: return Vector2.zero;
        }
    }
}
}