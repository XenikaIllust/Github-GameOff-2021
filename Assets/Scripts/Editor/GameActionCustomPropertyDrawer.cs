using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer( typeof( GameAction ) )]
public class GameActionCustomPropertyDrawer : PropertyDrawer {
    public override float GetPropertyHeight( SerializedProperty property, GUIContent label ) {
        return EditorGUIUtility.singleLineHeight * 5 + 6;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        float totalHeight = 0;

        EditorGUI.BeginProperty( position, label, property );
        EditorGUI.PrefixLabel( position, label ); 

        totalHeight += 18;

        EditorGUI.indentLevel++;

        SerializedProperty gameActionBlockProperty = property.FindPropertyRelative("GameActionBlock");
        var gameActionBlockRect = new Rect( position.x, position.y + totalHeight, position.width, 16 );
        EditorGUI.PropertyField( gameActionBlockRect, gameActionBlockProperty );
        
        totalHeight += EditorGUI.GetPropertyHeight( gameActionBlockProperty, GUIContent.none );

        System.Type gameActionBlockType = gameActionBlockProperty.objectReferenceValue?.GetType();
        if(gameActionBlockType == typeof(InflictDamageAction)) {
            SerializedProperty statIdProperty = property.FindPropertyRelative("StatId");
            var statIdRect = new Rect( position.x, position.y + totalHeight, position.width, 16 );
            EditorGUI.PropertyField( statIdRect, statIdProperty );

            totalHeight += EditorGUI.GetPropertyHeight( statIdProperty, GUIContent.none );
        }
        else if(gameActionBlockType == typeof(InflictHealAction)) {
            SerializedProperty statIdProperty = property.FindPropertyRelative("StatId");
            var statIdRect = new Rect( position.x, position.y + totalHeight, position.width, 16 );
            EditorGUI.PropertyField( statIdRect, statIdProperty );

            totalHeight += EditorGUI.GetPropertyHeight( statIdProperty, GUIContent.none );
        }
        else if(gameActionBlockType == typeof(ModifyTurnRateAction)) {
            SerializedProperty statIdProperty = property.FindPropertyRelative("StatId");
            var statIdRect = new Rect( position.x, position.y + totalHeight, position.width, 16 );
            EditorGUI.PropertyField( statIdRect, statIdProperty );

            totalHeight += EditorGUI.GetPropertyHeight( statIdProperty, GUIContent.none );
        }
        else if(gameActionBlockType == typeof(AimActionBlock)) {
            SerializedProperty statIdProperty = property.FindPropertyRelative("StatId");
            var statIdRect = new Rect( position.x, position.y + totalHeight, position.width, 16 );
            EditorGUI.PropertyField( statIdRect, statIdProperty );

            totalHeight += EditorGUI.GetPropertyHeight( statIdProperty, GUIContent.none );
        }
        else if(gameActionBlockType == typeof(PointVFXActionBlock)) {
            SerializedProperty vfxNameProperty = property.FindPropertyRelative("VFXName");
            var vfxNameRect = new Rect( position.x, position.y + totalHeight, position.width, 16 );
            EditorGUI.PropertyField( vfxNameRect, vfxNameProperty );

            totalHeight += EditorGUI.GetPropertyHeight( vfxNameProperty, GUIContent.none );

            SerializedProperty vfxPointIdProperty = property.FindPropertyRelative("VFXPointId");
            var vfxPointIdRect = new Rect( position.x, position.y + totalHeight, position.width, 16 );
            EditorGUI.PropertyField( vfxPointIdRect, vfxPointIdProperty );

            totalHeight += EditorGUI.GetPropertyHeight( vfxPointIdProperty, GUIContent.none );

            SerializedProperty statIdProperty = property.FindPropertyRelative("StatId");
            statIdProperty.stringValue = "";

            totalHeight += EditorGUI.GetPropertyHeight( statIdProperty, GUIContent.none );
        }
        else if(gameActionBlockType == typeof(AOEVFXActionBlock)) {
            SerializedProperty vfxNameProperty = property.FindPropertyRelative("VFXName");
            var vfxNameRect = new Rect( position.x, position.y + totalHeight, position.width, 16 );
            EditorGUI.PropertyField( vfxNameRect, vfxNameProperty );

            totalHeight += EditorGUI.GetPropertyHeight( vfxNameProperty, GUIContent.none );

            SerializedProperty vfxPointIdProperty = property.FindPropertyRelative("VFXPointId");
            var vfxPointIdRect = new Rect( position.x, position.y + totalHeight, position.width, 16 );
            EditorGUI.PropertyField( vfxPointIdRect, vfxPointIdProperty );

            totalHeight += EditorGUI.GetPropertyHeight( vfxPointIdProperty, GUIContent.none );

            SerializedProperty statIdProperty = property.FindPropertyRelative("StatId");
            statIdProperty.stringValue = "";

            totalHeight += EditorGUI.GetPropertyHeight( statIdProperty, GUIContent.none );
        }
        else if(gameActionBlockType == typeof(LineVFXActionBlock)) {
            
        }
        else if(gameActionBlockType == typeof(UnitVFXActionBlock)) {
            SerializedProperty vfxNameProperty = property.FindPropertyRelative("VFXName");
            var vfxNameRect = new Rect( position.x, position.y + totalHeight, position.width, 16 );
            EditorGUI.PropertyField( vfxNameRect, vfxNameProperty );

            totalHeight += EditorGUI.GetPropertyHeight( vfxNameProperty, GUIContent.none );
        }
        else if(gameActionBlockType == typeof(ArcIndicatorVFXActionBlock)) {
            SerializedProperty vfxNameProperty = property.FindPropertyRelative("VFXName");
            var vfxNameRect = new Rect( position.x, position.y + totalHeight, position.width, 16 );
            EditorGUI.PropertyField( vfxNameRect, vfxNameProperty );

            totalHeight += EditorGUI.GetPropertyHeight( vfxNameProperty, GUIContent.none );

            SerializedProperty vfxPointIdProperty = property.FindPropertyRelative("VFXPointId");
            var vfxPointIdRect = new Rect( position.x, position.y + totalHeight, position.width, 16 );
            EditorGUI.PropertyField( vfxPointIdRect, vfxPointIdProperty );

            totalHeight += EditorGUI.GetPropertyHeight( vfxPointIdProperty, GUIContent.none );

            SerializedProperty timeToLiveProperty = property.FindPropertyRelative("TimeToLive");
            var timeToLiveRect = new Rect( position.x, position.y + totalHeight, position.width, 16 );
            EditorGUI.PropertyField( timeToLiveRect, timeToLiveProperty );

            totalHeight += EditorGUI.GetPropertyHeight( timeToLiveProperty, GUIContent.none );
        }
        else if(gameActionBlockType == typeof(AOEIndicatorVFXActionBlock)) {
            SerializedProperty vfxNameProperty = property.FindPropertyRelative("VFXName");
            var vfxNameRect = new Rect( position.x, position.y + totalHeight, position.width, 16 );
            EditorGUI.PropertyField( vfxNameRect, vfxNameProperty );

            totalHeight += EditorGUI.GetPropertyHeight( vfxNameProperty, GUIContent.none );

            SerializedProperty vfxPointIdProperty = property.FindPropertyRelative("VFXPointId");
            var vfxPointIdRect = new Rect( position.x, position.y + totalHeight, position.width, 16 );
            EditorGUI.PropertyField( vfxPointIdRect, vfxPointIdProperty );

            totalHeight += EditorGUI.GetPropertyHeight( vfxPointIdProperty, GUIContent.none );

            SerializedProperty timeToLiveProperty = property.FindPropertyRelative("TimeToLive");
            var timeToLiveRect = new Rect( position.x, position.y + totalHeight, position.width, 16 );
            EditorGUI.PropertyField( timeToLiveRect, timeToLiveProperty );

            totalHeight += EditorGUI.GetPropertyHeight( timeToLiveProperty, GUIContent.none );
        }
        else if(gameActionBlockType == typeof(LineIndicatorVFXActionBlock)) {
            SerializedProperty vfxNameProperty = property.FindPropertyRelative("VFXName");
            var vfxNameRect = new Rect( position.x, position.y + totalHeight, position.width, 16 );
            EditorGUI.PropertyField( vfxNameRect, vfxNameProperty );

            totalHeight += EditorGUI.GetPropertyHeight( vfxNameProperty, GUIContent.none );

            SerializedProperty timeToLiveProperty = property.FindPropertyRelative("TimeToLive");
            var timeToLiveRect = new Rect( position.x, position.y + totalHeight, position.width, 16 );
            EditorGUI.PropertyField( timeToLiveRect, timeToLiveProperty );

            totalHeight += EditorGUI.GetPropertyHeight( timeToLiveProperty, GUIContent.none );
        }
        else if(gameActionBlockType == typeof(SFXActionBlock)) {
            SerializedProperty sfxNameProperty = property.FindPropertyRelative("SFXName");
            var sfxNameRect = new Rect( position.x, position.y + totalHeight, position.width, 16 );
            EditorGUI.PropertyField( sfxNameRect, sfxNameProperty );

            totalHeight += EditorGUI.GetPropertyHeight( sfxNameProperty, GUIContent.none );
        }
        else if(gameActionBlockType == typeof(FireProjectileAction)) {
            SerializedProperty projectileNameProperty = property.FindPropertyRelative("ProjectileName");
            var projectileNameRect = new Rect( position.x, position.y + totalHeight, position.width, 16 );
            EditorGUI.PropertyField( projectileNameRect, projectileNameProperty );

            totalHeight += EditorGUI.GetPropertyHeight( projectileNameProperty, GUIContent.none );

            SerializedProperty projectileStatIdProperty = property.FindPropertyRelative("ProjectileStatId");
            var projectileStatIdRect = new Rect( position.x, position.y + totalHeight, position.width, 16 );
            EditorGUI.PropertyField( projectileStatIdRect, projectileStatIdProperty );

            totalHeight += EditorGUI.GetPropertyHeight( projectileStatIdProperty, GUIContent.none );

            SerializedProperty timeToLiveProperty = property.FindPropertyRelative("TimeToLive");
            var timeToLiveRect = new Rect( position.x, position.y + totalHeight, position.width, 16 );
            EditorGUI.PropertyField( timeToLiveRect, timeToLiveProperty );

            totalHeight += EditorGUI.GetPropertyHeight( timeToLiveProperty, GUIContent.none );
        }
        else if(gameActionBlockType == typeof(AnimationActionBlock)) {
            SerializedProperty animationTriggerNameProperty = property.FindPropertyRelative("AnimationTriggerName");
            var animationTriggerNameRect = new Rect( position.x, position.y + totalHeight, position.width, 16 );
            EditorGUI.PropertyField( animationTriggerNameRect, animationTriggerNameProperty );

            totalHeight += EditorGUI.GetPropertyHeight( animationTriggerNameProperty, GUIContent.none );
        }

        EditorGUI.indentLevel--;

        EditorGUI.EndProperty();
    }

    void ClearAllFields() {
        // TODO
    }
}
