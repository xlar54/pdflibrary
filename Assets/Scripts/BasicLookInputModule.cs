using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class BasicLookInputModule : BaseInputModule
{

    public const int kLookId = -3;
    public string submitButtonName = "Fire1";
    public string controlAxisName = "Horizontal";
    private PointerEventData lookData;
    public Camera cam;

    // use screen midpoint as locked pointer location, enabling look location to be the "mouse"
    private PointerEventData GetLookPointerEventData()
    {
        Vector2 lookPosition;
        lookPosition.x = cam.pixelWidth / 2; // Screen.width / 2;
        lookPosition.y = cam.pixelHeight / 2; // Screen.height / 2;
        if (lookData == null)
        {
            lookData = new PointerEventData(eventSystem);
        }
        lookData.Reset();
        lookData.delta = Vector2.zero;
        lookData.position = lookPosition;

        lookData.scrollDelta = Vector2.zero;
        eventSystem.RaycastAll(lookData, m_RaycastResultCache);
        lookData.pointerCurrentRaycast = FindFirstRaycast(m_RaycastResultCache);
        m_RaycastResultCache.Clear();
        return lookData;
    }

    private bool SendUpdateEventToSelectedObject()
    {
        if (eventSystem.currentSelectedGameObject == null)
            return false;
        BaseEventData data = GetBaseEventData();
        ExecuteEvents.Execute(eventSystem.currentSelectedGameObject, data, ExecuteEvents.updateSelectedHandler);
        return data.used;
    }

    public override void Process()
    {
        // send update events if there is a selected object - this is important for InputField to receive keyboard events
        SendUpdateEventToSelectedObject();
        PointerEventData lookData = GetLookPointerEventData();
        // use built-in enter/exit highlight handler
        HandlePointerExitAndEnter(lookData, lookData.pointerCurrentRaycast.gameObject);


        if (lookData.pointerCurrentRaycast.gameObject != null)
        {
            GameObject go = lookData.pointerCurrentRaycast.gameObject.transform.parent.gameObject;


            Button b = go.GetComponent<Button>();

            if (b != null)
            {
                ColorBlock cb = b.colors;
                //cb.normalColor = Color.blue;
                cb.highlightedColor = Color.yellow;
                b.colors = cb;
            }

            Dropdown dd = go.GetComponent<Dropdown>();
            if (dd != null)
            {
                ColorBlock cb = dd.colors;
                //cb.normalColor = Color.blue;
                cb.highlightedColor = Color.yellow;
                dd.colors = cb;
            }

            Scrollbar sb = go.GetComponent<Scrollbar>();
            if (sb != null)
            {
                ColorBlock cb = sb.colors;
                //cb.normalColor = Color.blue;
                cb.highlightedColor = Color.yellow;
                sb.colors = cb;
            }
        }

        if (Input.GetButtonDown(submitButtonName))
        {
            eventSystem.SetSelectedGameObject(null);
            if (lookData.pointerCurrentRaycast.gameObject != null)
            {
                GameObject go = lookData.pointerCurrentRaycast.gameObject.transform.parent.gameObject;
                //Button b = go.GetComponent<Button>();
                //ColorBlock cb = b.colors;
                //cb.normalColor = Color.blue;
                //b.colors = cb;


                GameObject newPressed = ExecuteEvents.ExecuteHierarchy(go, lookData, ExecuteEvents.submitHandler);
                if (newPressed == null)
                {
                    // submit handler not found, try select handler instead
                    newPressed = ExecuteEvents.ExecuteHierarchy(go, lookData, ExecuteEvents.pointerClickHandler); // selectHandler);
                }
                if (newPressed != null)
                {
                    eventSystem.SetSelectedGameObject(newPressed);
                }
            }
        }
        if (eventSystem.currentSelectedGameObject && controlAxisName != null && controlAxisName != "")
        {
            float newVal = Input.GetAxis(controlAxisName);
            if (newVal > 0.01f || newVal < -0.01f) {
                AxisEventData axisData = GetAxisEventData(newVal, 0.0f, 0.0f);
                ExecuteEvents.Execute(eventSystem.currentSelectedGameObject, axisData, ExecuteEvents.moveHandler);
            }
        }
    }
}