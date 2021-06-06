using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragbleImage : MonoBehaviour, IDragHandler, IPointerDownHandler
{
    public NewBehaviourScript osnovnoySkript;
    public Canvas canvos;
    public RectTransform dragImage;
    int inIndexObject;//индекс того на что навели
    public Transform emptyObject;//пустышка которая создается в момент хватания
    Transform NewCriateObject;//UI объект который создали новый
    GameObject InputUiObject;//UI объект который схватили
    string nameAdress;//адрес UI обьекта
    Items BuferItems = null;//
    bool enabledd = false;
   
   public void OnPointerDown(PointerEventData eventData)
    {       
         if(eventData.pointerEnter.transform.parent.transform.Find("InputFieldName"))
        {
            InputUiObject = eventData.pointerEnter.transform.parent.gameObject;
            nameAdress = InputUiObject.transform.Find("InputFieldName").GetComponent<InputField>().text;

            dragImage.position = eventData.pointerEnter.transform.position;
            dragImage.GetChild(0).GetChild(0).GetComponent<InputField>().text = nameAdress;
            dragImage.SetAsLastSibling();
        
       
            if(InputUiObject.name == "Кнопка(Clone)")
            {
                //получаем индекс обьекта что схватили
                inIndexObject = int.Parse(eventData.pointerEnter.transform.parent.GetChild(0).GetComponent<Text>().text);
                
                //создаем пустышку
                NewCriateObject = Instantiate(emptyObject);
                //и наследуем ее в рейс
                NewCriateObject.SetParent(eventData.pointerEnter.transform.parent.parent);

                NewCriateObject.SetSiblingIndex(inIndexObject);
                
                //UI объект который схватили переносим в конец списка
                InputUiObject.transform.SetAsLastSibling();
                
                if(NewCriateObject.parent.parent.parent.name == "Меню Рейс №1")
                {
                    PolaReisUpdate(osnovnoySkript.polaReis, inIndexObject, true);
                }
                else 
                {
                    PolaReisUpdate(osnovnoySkript.polaReis2, inIndexObject, true);
                }
                
                //переносим иконку с адресом в самый конец всех элементов 
                AlpfaCanalOff(InputUiObject, false);
            }
        }
    }
    
 

    public void OnDrag(PointerEventData eventData)
    {
        /*if(NewCriateObject && eventData.pointerCurrentRaycast.gameObject && eventData.pointerCurrentRaycast.gameObject.transform.name == "Меню Рейс №1" ||
           NewCriateObject && eventData.pointerCurrentRaycast.gameObject && eventData.pointerCurrentRaycast.gameObject.transform.name == "Меню Рейс №2")
        {
            string acs1;
            string acs2;
            
            acs1 = NewCriateObject.parent.parent.parent.name;
            acs2 = eventData.pointerCurrentRaycast.gameObject.transform.name;
            
            if(acs1 == acs2)
            {
               // enabledd = true;
            }
           // else enabledd = false;
        }*/
        

        if(NewCriateObject && eventData.pointerEnter.transform.parent && eventData.pointerEnter.transform.parent.name == "Кнопка(Clone)" )
        {
            //получаем индекс обьекта что схватили
            inIndexObject = int.Parse(eventData.pointerEnter.transform.parent.GetChild(0).GetComponent<Text>().text);
            //переносим пустышку с авдресом в место индекса 
            NewCriateObject.transform.SetSiblingIndex(inIndexObject);
            //передаем пустышке номер места куда поставить
            NewCriateObject.GetChild(0).GetComponent<Text>().text = inIndexObject.ToString();
        }
        
        dragImage.anchoredPosition += eventData.delta / canvos.scaleFactor;
    }
    

    void Update ()
    {
        if(Input.GetMouseButtonUp(0))
        {
            if(NewCriateObject && NewCriateObject.parent.parent.parent)
            {
                string acs1 = NewCriateObject.parent.parent.parent.name;
                string acs2 = osnovnoySkript.acse;
                //Debug.Log("откуда= " + acs1 + " " + "где= " + acs2 + "   " + enabledd);
                if(acs1 == acs2)
                {
                    enabledd = true;
                }
                else
                {
                    enabledd = false;
                   // if(InputUiObject)AlpfaCanalOff(InputUiObject, true);  
                } 
            }
            dragImage.anchoredPosition = new Vector2(1000,1000);
            dragImage.GetChild(0).GetChild(0).GetComponent<InputField>().text ="";
            
            if(InputUiObject && InputUiObject.name == "Кнопка(Clone)" && enabledd)
            {
                 //UI объект который схватили переносим в место индекса
                InputUiObject.transform.SetSiblingIndex(inIndexObject);
                
                if(NewCriateObject.parent.parent.parent.name == "Меню Рейс №1")
                {
                   // Debug.Log("polaReis " + enabledd);
                    PolaReisUpdate(osnovnoySkript.polaReis, inIndexObject, false);
                }
                else
                {
                   // Debug.Log("polaReis2" + enabledd);
                    PolaReisUpdate(osnovnoySkript.polaReis2, inIndexObject, false);
                }
                
            }
            if(InputUiObject && InputUiObject.name == "Кнопка(Clone)")
            {
                AlpfaCanalOff(InputUiObject, true); 
                InputUiObject = null;
                if(NewCriateObject)
                {
                    Destroy(NewCriateObject.gameObject);
                    NewCriateObject = null;
                }
            }
            enabledd = false;
        }
    }

     
    void PolaReisUpdate(List<Items> array,int index ,  bool OnEnabled)
    {
        if(OnEnabled)
        {
            //делаем копию экземпляра массива в тот же класс Items
            BuferItems = array[index];
            //удаляем экземпляр из масива
            array.RemoveAt(index);
            //создаем новый в конце экземпляр в массиве только в конце
            array.Add(BuferItems);
            
            //перезаполняем номаре UI обьектов для того чтобы можно было бы поставить на нулевую позицию
            for (int i = 0; i < array.Count; i++)
            {
                array[i].Polatrans.GetChild(0).GetComponent<Text>().text = i.ToString();
            }
        }
        else
        {
            //вставляем новый экземпляр с местом индекса в массив
            array.Insert(index, BuferItems);
            
            //удаляем последний экземпляр из масива
            array.RemoveAt(array.Count -1 );
            //пересчитывает индекс и устанавливает в UI
            for (int i = 0; i < array.Count; i++)
            {
                array[i].Polatrans.GetChild(0).GetComponent<Text>().text = i.ToString();
            }
        }
        
    }
    void AlpfaCanalOff(GameObject InputUiObjectOff, bool OnEnabled)
    {
        Color alpfaCanal = InputUiObjectOff.GetComponent<Image>().color;
        if(OnEnabled)
        {
            alpfaCanal.a = 255f;
            InputUiObjectOff.GetComponent<Image>().color = alpfaCanal;
        }else 
        {
            alpfaCanal.a = 0;
            InputUiObjectOff.GetComponent<Image>().color = alpfaCanal;
        }
        
        alpfaCanal = InputUiObjectOff.transform.GetChild(0).GetComponent<Text>().color;
        if(OnEnabled)
        {
           // alpfaCanal.r = 0;
           // alpfaCanal.g = 0;
          //  alpfaCanal.b = 0;
            alpfaCanal.a = 255;
            InputUiObjectOff.transform.GetChild(0).GetComponent<Text>().color = alpfaCanal;
        }else 
        {
            //alpfaCanal.r = 255;
            //alpfaCanal.g = 255;
            //alpfaCanal.b = 255;
            alpfaCanal.a = 0;
            InputUiObjectOff.transform.GetChild(0).GetComponent<Text>().color = alpfaCanal;
        }

        alpfaCanal = InputUiObjectOff.transform.GetChild(1).Find("Text").GetComponent<Text>().color;
        if(OnEnabled)
        {
          //  alpfaCanal.r = 0;
          //  alpfaCanal.g = 0;
          //  alpfaCanal.b = 0;
            alpfaCanal.a = 255;
            InputUiObjectOff.transform.GetChild(1).Find("Text").GetComponent<Text>().color = alpfaCanal;
        }else 
        {
           // alpfaCanal.r = 255;
           // alpfaCanal.g = 255;
          //  alpfaCanal.b = 255;
            alpfaCanal.a = 0;
            InputUiObjectOff.transform.GetChild(1).Find("Text").GetComponent<Text>().color = alpfaCanal;
        }

        alpfaCanal = InputUiObjectOff.transform.GetChild(1).GetComponent<Image>().color;
        if(OnEnabled)
        {
            alpfaCanal.a = 255;
            InputUiObjectOff.transform.GetChild(1).GetComponent<Image>().color = alpfaCanal;
        }else 
        {
           
            alpfaCanal.a = 0;
            InputUiObjectOff.transform.GetChild(1).GetComponent<Image>().color = alpfaCanal;
        }

        InputUiObjectOff.transform.GetChild(2).gameObject.SetActive(OnEnabled);
        InputUiObjectOff.transform.GetChild(3).gameObject.SetActive(OnEnabled);
        InputUiObjectOff.transform.GetChild(4).gameObject.SetActive(OnEnabled);
        InputUiObjectOff.transform.GetChild(5).gameObject.SetActive(OnEnabled);
        InputUiObjectOff.transform.GetChild(6).gameObject.SetActive(OnEnabled);
        InputUiObjectOff.transform.GetChild(7).gameObject.SetActive(OnEnabled);
        InputUiObjectOff.transform.GetChild(8).gameObject.SetActive(OnEnabled);
        InputUiObjectOff.transform.GetChild(9).gameObject.SetActive(OnEnabled);
        InputUiObjectOff.transform.GetChild(10).gameObject.SetActive(OnEnabled);
        InputUiObjectOff.transform.GetChild(11).gameObject.SetActive(OnEnabled);
    }

    
}

