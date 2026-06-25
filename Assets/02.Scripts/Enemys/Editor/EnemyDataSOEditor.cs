#if UNITY_EDITOR
using _02.Scripts.CardSystem.Cards.ActionCards;
using UnityEditor;
using UnityEngine;

namespace _02.Scripts.Enemys
{
    [CustomEditor(typeof(EnemyDataSO))]
    public class EnemyDataSOEditor : UnityEditor.Editor
    {
        private const int DefaultSlotCount = 4;
        private const float SlotHeight = 120f;
        private const float SlotSpacing = 6f;

        private int _selectedSlot;

        private GUIStyle _headerStyle;
        private GUIStyle _nameStyle;
        private GUIStyle _valueStyle;
        private GUIStyle _emptyStyle;

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EnsureStyles();

            using (new EditorGUI.DisabledScope(true))
                EditorGUILayout.PropertyField(serializedObject.FindProperty("m_Script"));

            // CardPlacements 를 제외한 나머지 필드는 기본 인스펙터로 그린다.
            DrawPropertiesExcluding(serializedObject,
                "m_Script",
                "CardPlacements", "<CardPlacements>k__BackingField",
                "RespawnTurnDelay", "<RespawnTurnDelay>k__BackingField",
                "MaxRespawnCount", "<MaxRespawnCount>k__BackingField",
                "RespawnPool", "<RespawnPool>k__BackingField");

            SerializedProperty listProp = FindProp(serializedObject, "CardPlacements");
            if (listProp != null)
            {
                EditorGUILayout.Space(8);
                EditorGUILayout.LabelField("적 카드 배치", EditorStyles.boldLabel);
                DrawSlotGrid(listProp);
                EditorGUILayout.Space(8);
                DrawSlotEditor(listProp);
            }

            EditorGUILayout.Space(12);
            DrawRespawnSection();

            serializedObject.ApplyModifiedProperties();
        }

        private void DrawSlotGrid(SerializedProperty listProp)
        {
            int slotCount = DefaultSlotCount;
            for (int i = 0; i < listProp.arraySize; i++)
            {
                int si = listProp.GetArrayElementAtIndex(i).FindPropertyRelative("SlotIndex").intValue;
                if (si + 1 > slotCount) slotCount = si + 1;
            }

            Rect row = GUILayoutUtility.GetRect(0, SlotHeight, GUILayout.ExpandWidth(true));
            float slotWidth = (row.width - SlotSpacing * (slotCount - 1)) / slotCount;

            for (int i = 0; i < slotCount; i++)
            {
                Rect r = new Rect(row.x + i * (slotWidth + SlotSpacing), row.y, slotWidth, SlotHeight);
                DrawSlot(r, i, listProp);
            }
        }

        private void DrawSlot(Rect rect, int slotIndex, SerializedProperty listProp)
        {
            SerializedProperty placement = FindPlacement(listProp, slotIndex);
            bool selected = slotIndex == _selectedSlot;
            var cardData = placement?.FindPropertyRelative("CardData").objectReferenceValue as ActionCardDataSO;

            Color bg = selected ? new Color(0.24f, 0.42f, 0.66f) : new Color(0.22f, 0.22f, 0.22f);
            EditorGUI.DrawRect(rect, bg);
            DrawBorder(rect, selected ? new Color(0.4f, 0.7f, 1f) : new Color(0.1f, 0.1f, 0.1f), selected ? 2f : 1f);

            Rect header = new Rect(rect.x, rect.y + 3, rect.width, 16);
            GUI.Label(header, $"슬롯 {slotIndex}", _headerStyle);

            if (cardData != null)
            {
                float iconSize = Mathf.Min(rect.width - 16f, 56f);
                Rect iconRect = new Rect(rect.x + (rect.width - iconSize) * 0.5f, rect.y + 22f, iconSize, iconSize);
                if (cardData.Icon != null)
                    DrawSprite(iconRect, cardData.Icon);
                else
                    EditorGUI.DrawRect(iconRect, new Color(0.15f, 0.15f, 0.15f));

                Rect nameRect = new Rect(rect.x + 2f, iconRect.yMax + 2f, rect.width - 4f, 14f);
                GUI.Label(nameRect, cardData.Name, _nameStyle);

                int atk = FindRel(placement, "AttackValue").intValue;
                int def = FindRel(placement, "DefenseValue").intValue;
                Rect valRect = new Rect(rect.x + 2f, nameRect.yMax, rect.width - 4f, 14f);
                GUI.Label(valRect, $"공 {atk} / 방 {def}", _valueStyle);
            }
            else
            {
                Rect empty = new Rect(rect.x, rect.y + rect.height * 0.5f - 10f, rect.width, 20f);
                GUI.Label(empty, "비어있음", _emptyStyle);
            }

            if (Event.current.type == EventType.MouseDown && rect.Contains(Event.current.mousePosition))
            {
                _selectedSlot = slotIndex;
                Event.current.Use();
                Repaint();
            }
        }

        private void DrawSlotEditor(SerializedProperty listProp)
        {
            EditorGUILayout.LabelField($"슬롯 {_selectedSlot} 설정", EditorStyles.boldLabel);
            SerializedProperty placement = FindPlacement(listProp, _selectedSlot);

            if (placement == null)
            {
                EditorGUILayout.HelpBox("비어있는 슬롯입니다. 카드를 지정하면 자동으로 추가됩니다.", MessageType.Info);
                var newCard = EditorGUILayout.ObjectField("카드", null, typeof(ActionCardDataSO), false) as ActionCardDataSO;
                if (newCard != null)
                {
                    int idx = listProp.arraySize;
                    listProp.arraySize++;
                    SerializedProperty el = listProp.GetArrayElementAtIndex(idx);
                    el.FindPropertyRelative("SlotIndex").intValue = _selectedSlot;
                    el.FindPropertyRelative("CardData").objectReferenceValue = newCard;
                    FindRel(el, "AttackValue").intValue = 0;
                    FindRel(el, "DefenseValue").intValue = 0;
                }
                return;
            }

            EditorGUILayout.PropertyField(placement.FindPropertyRelative("CardData"), new GUIContent("카드"));
            EditorGUILayout.PropertyField(FindRel(placement, "AttackValue"), new GUIContent("공격력"));
            EditorGUILayout.PropertyField(FindRel(placement, "DefenseValue"), new GUIContent("방어력"));

            EditorGUILayout.Space(4);
            Color prev = GUI.backgroundColor;
            GUI.backgroundColor = new Color(0.9f, 0.5f, 0.5f);
            if (GUILayout.Button("이 슬롯 비우기"))
                RemovePlacement(listProp, _selectedSlot);
            GUI.backgroundColor = prev;
        }

        private void DrawRespawnSection()
        {
            EditorGUILayout.LabelField("리스폰 설정", EditorStyles.boldLabel);

            SerializedProperty delay = FindProp(serializedObject, "RespawnTurnDelay");
            SerializedProperty maxCount = FindProp(serializedObject, "MaxRespawnCount");
            SerializedProperty pool = FindProp(serializedObject, "RespawnPool");

            if (delay != null) EditorGUILayout.PropertyField(delay, new GUIContent("리스폰 대기 턴"));
            if (maxCount != null) EditorGUILayout.PropertyField(maxCount, new GUIContent("최대 리스폰 횟수"));
            if (pool != null)
            {
                EditorGUILayout.Space(2);
                EditorGUILayout.PropertyField(pool, new GUIContent("리스폰 카드 풀"), true);
                EditorGUILayout.HelpBox("적 카드가 죽으면 대기 턴 후, 이 풀에서 랜덤으로 하나를 골라 빈 슬롯에 소환합니다. (최대 리스폰 횟수만큼)", MessageType.None);
            }
        }

        private static SerializedProperty FindPlacement(SerializedProperty listProp, int slotIndex)
        {
            for (int i = 0; i < listProp.arraySize; i++)
            {
                SerializedProperty el = listProp.GetArrayElementAtIndex(i);
                if (el.FindPropertyRelative("SlotIndex").intValue == slotIndex) return el;
            }
            return null;
        }

        private static void RemovePlacement(SerializedProperty listProp, int slotIndex)
        {
            for (int i = 0; i < listProp.arraySize; i++)
            {
                if (listProp.GetArrayElementAtIndex(i).FindPropertyRelative("SlotIndex").intValue == slotIndex)
                {
                    listProp.DeleteArrayElementAtIndex(i);
                    return;
                }
            }
        }

        private static void DrawSprite(Rect rect, Sprite sprite)
        {
            Texture2D tex = sprite.texture;
            if (tex == null) return;

            Rect tr = sprite.textureRect;
            Rect texCoords = new Rect(
                tr.x / tex.width,
                tr.y / tex.height,
                tr.width / tex.width,
                tr.height / tex.height);
            GUI.DrawTextureWithTexCoords(rect, tex, texCoords);
        }

        private static void DrawBorder(Rect r, Color color, float t)
        {
            EditorGUI.DrawRect(new Rect(r.x, r.y, r.width, t), color);
            EditorGUI.DrawRect(new Rect(r.x, r.yMax - t, r.width, t), color);
            EditorGUI.DrawRect(new Rect(r.x, r.y, t, r.height), color);
            EditorGUI.DrawRect(new Rect(r.xMax - t, r.y, t, r.height), color);
        }

        // [field: SerializeField] 자동 프로퍼티의 백킹필드(<Name>k__BackingField)와 일반 필드를 모두 처리한다.
        private static SerializedProperty FindProp(SerializedObject so, string name)
            => so.FindProperty(name) ?? so.FindProperty($"<{name}>k__BackingField");

        private static SerializedProperty FindRel(SerializedProperty prop, string name)
            => prop.FindPropertyRelative(name) ?? prop.FindPropertyRelative($"<{name}>k__BackingField");

        private void EnsureStyles()
        {
            if (_headerStyle != null) return;

            _headerStyle = new GUIStyle(EditorStyles.boldLabel) { alignment = TextAnchor.MiddleCenter };
            _headerStyle.normal.textColor = Color.white;

            _nameStyle = new GUIStyle(EditorStyles.miniLabel) { alignment = TextAnchor.MiddleCenter };
            _nameStyle.normal.textColor = Color.white;

            _valueStyle = new GUIStyle(EditorStyles.miniLabel) { alignment = TextAnchor.MiddleCenter };
            _valueStyle.normal.textColor = new Color(0.85f, 0.85f, 0.85f);

            _emptyStyle = new GUIStyle(EditorStyles.miniLabel) { alignment = TextAnchor.MiddleCenter };
            _emptyStyle.normal.textColor = new Color(0.6f, 0.6f, 0.6f);
        }
    }
}
#endif
