using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.IO;//позволяет обращаться и создавать  тектовые документы
//using Microsoft.Office.Interop.Excel;
//using Excel = Microsoft.Office.Interop.Excel;


public class DragWindow : MyClass, IDragHandler,IPointerDownHandler,IBeginDragHandler,IEndDragHandler, IDropHandler
{
    
    NewBehaviourScript osnovnoySkript;
    public Transform PrefabNomenReis;
    RectTransform Windo;
    RectTransform DragRectTransform;
    public RectTransform reis;
    Canvas canvos;
    public CanvasGroup CanvosGroupe;
    bool ondrag=false;
    bool ondscale = false;
    bool ondropReis = false;
    string stopName1;
    string stopName2;
    public bool scaleble = true;
    
    PointerEventData eventDatad;

    void Start()
    {
        Transform trans = transform.parent;
        osnovnoySkript = trans.Find("Главное Меню").GetComponent<NewBehaviourScript>();
        if (!DragRectTransform)
        {
            Windo = transform.GetComponent<RectTransform>();
            DragRectTransform = transform.Find("drag").GetComponent<RectTransform>();
          
            if (!canvos)
            {
                
                while(trans)
                {
                    
                    canvos = trans.GetComponent<Canvas>();
                    if (canvos) break;
                    trans = trans.parent;
                  
                }
                
            }
        }
    }
   
    public void OnBeginDrag(PointerEventData eventData)
    {
      
        
        //--------------------------------раздел таскания и растягивания менюшик--------------------------------
        
        stopName1 = transform.name;
        
        if (reis && eventData.pointerEnter.transform.name == reis.transform.name)
        {
            reis.transform.SetParent(canvos.transform);
            CanvosGroupe.blocksRaycasts = false;
            ondropReis = true;
           
        }
        if (eventData.pointerEnter.transform.name == DragRectTransform.transform.name)
        {
            ondrag = true;
        }
        if (eventData.pointerEnter.transform.name == Windo.transform.name && scaleble)
        {
            ondscale = true;
        }
        //------------------------------------------------------------------------------------------------------
    }

    public void OnDrag(PointerEventData eventData)
    {
        
        stopName2 = transform.name;
        
        if (ondrag)
        {
            Windo.anchoredPosition += eventData.delta / canvos.scaleFactor;
        }

        if (ondscale)
        {
            Vector2 pos = DragRectTransform.anchoredPosition;
            pos.x = eventData.delta.x;
            pos.y = eventData.delta.y;

            if (Windo.GetComponent<RectTransform>().sizeDelta.y >= 200f)
            {
                Windo.GetComponent<RectTransform>().sizeDelta += new Vector2( 0, -pos.y ) / canvos.scaleFactor;
                Windo.GetComponent<RectTransform>().position += new Vector3( 0, pos.y / 2 / canvos.scaleFactor );
            }
            else
            {
                Windo.GetComponent<RectTransform>().sizeDelta = new Vector2(608.4f,200) / canvos.scaleFactor; 
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if(CanvosGroupe) CanvosGroupe.blocksRaycasts = true;
        ondrag = false;
        ondscale = false;
        ondropReis = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {       
        Windo.SetAsLastSibling();
    }

    public void OnDrop(PointerEventData eventData)
    {
        Transform bufer;
        bool ecsess1=true;
        bool ecsess2=true;
        osnovnoySkript.acse = transform.name.ToString();
        
        if (eventData.pointerDrag.transform.parent.name == "Кнопка" && transform.name == "Меню Рейс №1"||
            eventData.pointerDrag.transform.parent.name == "Кнопка" && transform.name == "Меню Рейс №2")
        {
            // устанавливаем индекс элемента которого схватиле у контрагентов
            int indexObject = int.Parse(eventData.pointerDrag.transform.parent.GetChild(0).GetComponent<Text>().text);

            for (int i = 0; i < osnovnoySkript.reis1Index; i++)
            {
                if(osnovnoySkript.polaReis[i].Adress == 
                   osnovnoySkript.Kontragent[indexObject].Adress)
                {
                    ecsess1=false;
                    StartCoroutine(routine: ReturnColor(osnovnoySkript.polaReis[i].Polatrans));
                }
            }

            for (int i = 0; i < osnovnoySkript.reis2Index; i++)
            {
                if(osnovnoySkript.polaReis2[i].Adress ==
                   osnovnoySkript.Kontragent[indexObject].Adress)
                {
                    ecsess2=false;
                    StartCoroutine(routine: ReturnColor(osnovnoySkript.polaReis2[i].Polatrans));
                }
            }

            if(ecsess1 && ecsess2 && transform.name == "Меню Рейс №1")
            {
                bufer = Instantiate(PrefabNomenReis);
                bufer.SetParent(transform.Find("Scrol/Content"));

                bufer.Find("Text").GetComponent<Text>().text = osnovnoySkript.reis1Index.ToString();
                
                osnovnoySkript.polaReis.Add(new Items());
                osnovnoySkript.polaReis[osnovnoySkript.polaReis.Count -1] = osnovnoySkript.ListCopy(osnovnoySkript.Kontragent, indexObject, osnovnoySkript.polaReis, osnovnoySkript.polaReis.Count -1, bufer);

                InsertTheDataObject(bufer, osnovnoySkript.polaReis ,osnovnoySkript.reis1Index);

                SetTheColorsDebt(osnovnoySkript.polaReis, osnovnoySkript.reis1Index);
                osnovnoySkript.reis1Index ++;
                
            }

            if(ecsess1 && ecsess2 && transform.name == "Меню Рейс №2")
            {
                bufer = Instantiate(PrefabNomenReis);
                bufer.SetParent(transform.Find("Scrol/Content"));
                
                bufer.Find("Text").GetComponent<Text>().text = osnovnoySkript.reis2Index.ToString();

                osnovnoySkript.polaReis2.Add(new Items());
                osnovnoySkript.polaReis2[osnovnoySkript.polaReis2.Count -1] = osnovnoySkript.ListCopy(osnovnoySkript.Kontragent, indexObject, osnovnoySkript.polaReis2, osnovnoySkript.polaReis2.Count -1, bufer);
                
                InsertTheDataObject(bufer, osnovnoySkript.polaReis2 ,osnovnoySkript.reis2Index);

                SetTheColorsDebt(osnovnoySkript.polaReis2, osnovnoySkript.reis2Index);
                osnovnoySkript.reis2Index ++;
            }
        }
       

        if (eventData.pointerDrag.transform.parent.name == "Кнопка(Clone)" && transform.name=="Меню Рейс №1" ||
            eventData.pointerDrag.transform.parent.name == "Кнопка(Clone)" && transform.name == "Меню Рейс №2")
        {
            string adress = eventData.pointerDrag.transform.parent.Find("InputFieldName").GetComponent<InputField>().text;
           
            bool acsessReis1 = true;
            bool acsessReis2 = true;
            
            if(transform.name=="Меню Рейс №2")//когда перетаскиваем в рейс 2
            {
                for (int i = 0; i < osnovnoySkript.polaReis2.Count; i++)
                {
                    if(adress == osnovnoySkript.polaReis2[i].Adress)
                    {
                        acsessReis2 = false;
                    }
                }
                if(acsessReis2)
                {
                    bufer = Instantiate(PrefabNomenReis);
                    bufer.name = "Кнопка(Clone)";
                    bufer.SetParent(transform.Find("Scrol/Content"));
                    for (int i = 0; i < osnovnoySkript.polaReis.Count ; i++)
                    {
                        if(adress == osnovnoySkript.polaReis[i].Adress)
                        {
                            // копируем данные из массива в UI второго массива
                            InsertTheDataObject(bufer, osnovnoySkript.polaReis ,i);
                            
                            osnovnoySkript.polaReis2.Add(new Items());
                            osnovnoySkript.polaReis2[osnovnoySkript.polaReis2.Count -1] = 
                            osnovnoySkript.ListCopy(osnovnoySkript.polaReis, i, osnovnoySkript.polaReis2, osnovnoySkript.polaReis2.Count -1);
                            
                            StartCoroutine(routine: osnovnoySkript.DestroyObjectAndArray(eventData.pointerDrag.transform.parent.gameObject, 
                            osnovnoySkript.polaReis,i));
                            
                            osnovnoySkript.reis1Index -- ;

                            bufer.Find("Text").GetComponent<Text>().text = osnovnoySkript.reis2Index.ToString();
                            osnovnoySkript.reis2Index ++;
                            
                            osnovnoySkript.polaReis2[osnovnoySkript.reis2Index-1].Polatrans=bufer;
                            osnovnoySkript.SetTheColorsObjects(bufer, osnovnoySkript.polaReis2, osnovnoySkript.reis2Index-1);
                            
                        }
                    }
                }
                
         
                osnovnoySkript.Podschet();
                osnovnoySkript.Save();
            }

            if(transform.name=="Меню Рейс №1")//когда перетаскиваем в рейс 1
            {
                for (int i = 0; i < osnovnoySkript.polaReis.Count; i++)
                {
                    if(adress == osnovnoySkript.polaReis[i].Adress)
                    {
                        acsessReis1 = false;
                    }
                }

                if(acsessReis1)
                {
                    bufer = Instantiate(PrefabNomenReis);
                    bufer.name = "Кнопка(Clone)";
                    bufer.SetParent(transform.Find("Scrol/Content"));
                    for (int i = 0; i < osnovnoySkript.polaReis2.Count ; i++)
                    {
                        if(adress == osnovnoySkript.polaReis2[i].Adress)
                        {
                            // копируем данные из массива в UI второго массива
                            InsertTheDataObject(bufer, osnovnoySkript.polaReis2 ,i);

                            osnovnoySkript.polaReis.Add(new Items());
                            osnovnoySkript.polaReis[osnovnoySkript.polaReis.Count -1] = 
                            osnovnoySkript.ListCopy(osnovnoySkript.polaReis2, i, osnovnoySkript.polaReis, osnovnoySkript.polaReis.Count -1);
                            
                            StartCoroutine(routine: osnovnoySkript.DestroyObjectAndArray(eventData.pointerDrag.transform.parent.gameObject,
                            osnovnoySkript.polaReis2,i));
                            
                            osnovnoySkript.reis2Index -- ;

                            bufer.Find("Text").GetComponent<Text>().text = osnovnoySkript.reis1Index.ToString();
                            osnovnoySkript.reis1Index ++;
                            
                            osnovnoySkript.polaReis[osnovnoySkript.reis1Index-1].Polatrans=bufer;
                            osnovnoySkript.SetTheColorsObjects(bufer, osnovnoySkript.polaReis, osnovnoySkript.reis1Index-1);
                            
                        }
                        
                    }
                }
                osnovnoySkript.Podschet();
                osnovnoySkript.Save();
            }
        }
    }

    void InsertTheDataObject(Transform bufer,List<Items> array, int index)
    {
        bufer.Find("InputFieldName").GetComponent<InputField>().text = array[index].Adress;
    
        bufer.Find("InputFieldTele").GetComponent<InputField>().text = array[index].telephone.ToString();

        bufer.Find("InputFieldDarinaPrice/TextPrice").GetComponent<Text>().text = "X "+ array[index].darina.ToString();
        if(array[index].darinaSum != 0)
        {
            bufer.Find("InputFieldDarinaPrice").GetComponent<InputField>().text = array[index].darinaSum.ToString();    
        } else bufer.Find("InputFieldDarinaPrice").GetComponent<InputField>().text = "";
        
        bufer.Find("InputFieldGKPrice/TextPrice").GetComponent<Text>().text = array[index].GornoyKrest.ToString();
        if(array[index].GornoyKrestSum != 0)
        {
            bufer.Find("InputFieldGKPrice").GetComponent<InputField>().text = array[index].GornoyKrestSum.ToString();
        } else  bufer.Find("InputFieldGKPrice").GetComponent<InputField>().text = "";
        
        bufer.Find("InputFieldDMPrice/TextPrice").GetComponent<Text>().text = "X " + array[index].Dombay.ToString();
        if(array[index].DombaySum != 0)
        {
            bufer.Find("InputFieldDMPrice").GetComponent<InputField>().text = array[index].DombaySum.ToString();
        } else  bufer.Find("InputFieldDMPrice").GetComponent<InputField>().text = "";
        
        bufer.Find("InputFieldGVPrice/TextPrice").GetComponent<Text>().text = "X " + array[index].GornoyVersh.ToString();
        if(array[index].GornoyVershSum != 0)
        {
            bufer.Find("InputFieldGVPrice").GetComponent<InputField>().text = array[index].GornoyVershSum.ToString();
        } else   bufer.Find("InputFieldGVPrice").GetComponent<InputField>().text = "";
        

        bufer.Find("InputFieldPIPrice/TextPrice").GetComponent<Text>().text = "X " + array[index].Pilegrim.ToString();
        if(array[index].PilegrimSum != 0)
        {
            Debug.Log("d");
           bufer.Find("InputFieldPIPrice").GetComponent<InputField>().text = array[index].PilegrimSum.ToString();
        } else   bufer.Find("InputFieldPIPrice").GetComponent<InputField>().text = "";
        

        bufer.Find("InputFieldKUPrice/TextPrice").GetComponent<Text>().text = "X " + array[index].Kubay.ToString();
        if(array[index].KubaySum != 0)
        {
           bufer.Find("InputFieldKUPrice").GetComponent<InputField>().text = array[index].KubaySum.ToString();
        } else   bufer.Find("InputFieldKUPrice").GetComponent<InputField>().text = "";
        

        bufer.Find("InputFieldTNPrice/TextPrice").GetComponent<Text>().text = "X " + array[index].Tanais.ToString();
        if(array[index].TanaisSum != 0)
        {
           bufer.Find("InputFieldTNPrice").GetComponent<InputField>().text = array[index].TanaisSum.ToString();
        } else   bufer.Find("InputFieldTNPrice").GetComponent<InputField>().text = "";
        
    }
    void SetTheColorsDebt(List<Items> array, int index)
    {
        if(array[index].isDebts && !array[index].isAlien)
        {
            array[index].Polatrans.GetChild(1).GetComponent<Image>().color =  new Color(255/255.0f,223/255.0f,1/255.0f);  
            array[index].Polatrans.GetChild(2).GetComponent<Image>().color =  new Color(255/255.0f,223/255.0f,1/255.0f); 
            array[index].Polatrans.GetChild(10).GetComponent<Image>().color =  new Color(255/255.0f,223/255.0f,1/255.0f);  
            array[index].Polatrans.GetChild(11).GetComponent<Image>().color =  new Color(255/255.0f,223/255.0f,1/255.0f);  
            osnovnoySkript.AddDebts(array[index]);
        }
        else
        if(!array[index].isDebts && array[index].isAlien)
        {                    
            array[index].Polatrans.GetChild(1).GetComponent<Image>().color =  new Color(255/255.0f,100/255.0f,100/255.0f);  
            array[index].Polatrans.GetChild(2).GetComponent<Image>().color =  new Color(255/255.0f,100/255.0f,100/255.0f); 
            array[index].Polatrans.GetChild(10).GetComponent<Image>().color =  new Color(255/255.0f,100/255.0f,100/255.0f);  
            array[index].Polatrans.GetChild(11).GetComponent<Image>().color =  new Color(255/255.0f,100/255.0f,100/255.0f);  
            osnovnoySkript.AddDebts(array[index]);
        }
    }
    
   
    string PrintAdress;
    string MassiveAdres;
    string PrintTele;
    String Masivetele;
    
    public void Print()//сохраняет в файл и открывает этот файл для печати-+
    {       
        int index=0;
       
        StreamWriter pr = new StreamWriter("SavePrint.ods");        

        pr.WriteLine("--Адрес--" + "     " + "--Телефон--" + "     " + "----ДР----" + "   " + "----ГК----" + "    "+ "----ДМ----"+"   "+"----ГВ----"+"   "+"----ПИЛ----"+"   "+"----КУБ----"+"   "+"----ТАН----"+"   "+"--Сумма--");
           
            string[] words;   
           
            for (int a = 0; a < osnovnoySkript.reis1Index; a++)
            {
                PrintAdress="";
                MassiveAdres="";
                PrintTele="";
                Masivetele="";
               

                MassiveAdres=osnovnoySkript.polaReis[index].Adress;
                words= MassiveAdres.Split(' ');               
                
                for(int z=0;z<words.Length;z++)
                {
                    PrintAdress+=words[z]+"⠀"; //<- тут скрытый символ
                    
                }
               
                Masivetele=osnovnoySkript.polaReis[index].telephone.ToString();
                words= Masivetele.Split(' ');  
                
                for(int z=0;z<words.Length;z++)
                {
                    PrintTele+=words[z]+"⠀"; //<- тут скрытый символ
                    
                }
                Debug.Log(PrintTele);
                string darina = osnovnoySkript.polaReis[index].darinaSum.ToString();
                string gornoy = osnovnoySkript.polaReis[index].GornoyKrestSum.ToString();
                string dombay = osnovnoySkript.polaReis[index].DombaySum.ToString();
                string gornoyVersh = osnovnoySkript.polaReis[index].GornoyVershSum.ToString();
                string pilegrim = osnovnoySkript.polaReis[index].PilegrimSum.ToString();
                string kubay = osnovnoySkript.polaReis[index].KubaySum.ToString();
                string tanais = osnovnoySkript.polaReis[index].TanaisSum.ToString();
                if(int.Parse(darina) < 1){darina = "⠀";}//<- тут скрытый символ
                if(int.Parse(gornoy) < 1){gornoy = "⠀";}//<- тут скрытый символ
                if(int.Parse(dombay) < 1){dombay = "⠀";}
                if(int.Parse(gornoyVersh) < 1){gornoyVersh = "⠀";}//<- тут скрытый символ
                if(int.Parse(pilegrim) < 1){pilegrim = "⠀";}//<- тут скрытый символ
                if(int.Parse(kubay) < 1){kubay = "⠀";}//<- тут скрытый символ
                if(int.Parse(tanais) < 1){tanais = "⠀";}//<- тут скрытый символ
                
                pr.WriteLine(PrintAdress + " "+PrintTele+" "+
                darina + " " + gornoy +" "+ dombay +" "+ gornoyVersh +" "+ pilegrim +" "+ kubay +" "+ tanais + 
                " " + osnovnoySkript.polaReis[index].Polatrans.GetChild(10).GetComponent<InputField>().text);
                index++;
            }
            pr.WriteLine(" ");
            pr.WriteLine("ЗП="+osnovnoySkript.reis1.GetChild(7).GetComponent<InputField>().text+" "+
            "Кол_ОбЩ="+osnovnoySkript.reis1.GetChild(15).GetComponent<InputField>().text+" "+
            osnovnoySkript.reis1.GetChild(8).GetComponent<InputField>().text+" "+
            osnovnoySkript.reis1.GetChild(9).GetComponent<InputField>().text+" "+
            osnovnoySkript.reis1.GetChild(10).GetComponent<InputField>().text+" "+
            osnovnoySkript.reis1.GetChild(11).GetComponent<InputField>().text+" "+
            osnovnoySkript.reis1.GetChild(12).GetComponent<InputField>().text+" "+
            osnovnoySkript.reis1.GetChild(13).GetComponent<InputField>().text+" "+
            osnovnoySkript.reis1.GetChild(14).GetComponent<InputField>().text+" "+
            osnovnoySkript.reis1.GetChild(5).GetComponent<InputField>().text);
       

        pr.Close();
        System.Diagnostics.Process.Start("SavePrint.ods");           

    }

    public void Print2()//сохраняет в файл и открывает этот файл для печати-+
    {       
        int index=0;
       
       
        StreamWriter pr = new StreamWriter("SavePrint.ods");
        
        pr.WriteLine("--Адрес--" + "     " + "--Телефон--" + "     " + "----ДР----" + "   " + "----ГК----" + "    "+ "----ДМ----"+"   "+"----ГВ----"+"   "+"----ПИЛ----"+"   "+"----КУБ----"+"   "+"----ТАН----"+"   "+"--Сумма--");
           
            string[] words;   
           
            for (int a = 0; a < osnovnoySkript.reis2Index; a++)
            {
                PrintAdress="";
                MassiveAdres="";
                PrintTele="";
                Masivetele="";
               

                MassiveAdres=osnovnoySkript.polaReis2[index].Adress;
                words= MassiveAdres.Split(' ');               
                
                for(int z=0;z<words.Length;z++)
                {
                    PrintAdress+=words[z]+"⠀";//<- тут скрытый символ
                    
                }
               
                Masivetele=osnovnoySkript.polaReis2[index].telephone.ToString();
                words= Masivetele.Split(' ');  
 
                for(int z=0;z<words.Length;z++)
                {
                    PrintTele+=words[z]+"⠀";//<- тут скрытый символ
                    
                }

                string darina = osnovnoySkript.polaReis2[index].darinaSum.ToString();
                string gornoy = osnovnoySkript.polaReis2[index].GornoyKrestSum.ToString();
                string dombay = osnovnoySkript.polaReis2[index].DombaySum.ToString();
                string gornoyVersh = osnovnoySkript.polaReis2[index].GornoyVershSum.ToString();
                string pilegrim = osnovnoySkript.polaReis2[index].PilegrimSum.ToString();
                string kubay = osnovnoySkript.polaReis2[index].KubaySum.ToString();
                string tanais = osnovnoySkript.polaReis2[index].TanaisSum.ToString();
                if(int.Parse(darina) < 1){darina = "⠀";}//<- тут скрытый символ
                if(int.Parse(gornoy) < 1){gornoy = "⠀";}//<- тут скрытый символ
                if(int.Parse(dombay) < 1){dombay = "⠀";}//<- тут скрытый символ
                if(int.Parse(gornoyVersh) < 1){gornoyVersh = "⠀";}//<- тут скрытый символ
                if(int.Parse(pilegrim) < 1){pilegrim = "⠀";}//<- тут скрытый символ
                if(int.Parse(kubay) < 1){kubay = "⠀";}//<- тут скрытый символ
                if(int.Parse(tanais) < 1){tanais = "⠀";}//<- тут скрытый символ
                
                pr.WriteLine(PrintAdress + " "+PrintTele+" "+
                darina + " " + gornoy +" "+ dombay +" "+ gornoyVersh +" "+ pilegrim +" "+ kubay +" "+ tanais + 
                " " + osnovnoySkript.polaReis2[index].Polatrans.GetChild(10).GetComponent<InputField>().text);
                index++;
            }
           
            pr.WriteLine(" ");
            pr.WriteLine("ЗП="+osnovnoySkript.reis2.GetChild(7).GetComponent<InputField>().text+" "+
            "Кол_ОбЩ="+osnovnoySkript.reis2.GetChild(15).GetComponent<InputField>().text+" "+
            osnovnoySkript.reis2.GetChild(8).GetComponent<InputField>().text+" "+
            osnovnoySkript.reis2.GetChild(9).GetComponent<InputField>().text+" "+
            osnovnoySkript.reis2.GetChild(10).GetComponent<InputField>().text+" "+
            osnovnoySkript.reis2.GetChild(11).GetComponent<InputField>().text+" "+
            osnovnoySkript.reis2.GetChild(12).GetComponent<InputField>().text+" "+
            osnovnoySkript.reis2.GetChild(13).GetComponent<InputField>().text+" "+
            osnovnoySkript.reis2.GetChild(14).GetComponent<InputField>().text+" "+
            osnovnoySkript.reis2.GetChild(5).GetComponent<InputField>().text


            );
       

        pr.Close();
        System.Diagnostics.Process.Start("SavePrint.ods");
        
        
    }
    public void exper()
    {
        
     /*   StreamWriter pr = new StreamWriter("SavePrint.xlsx",true);

        pr.WriteLine(1);
        pr.WriteLine(2);

        pr.Close();
        System.Diagnostics.Process.Start("SavePrint.xlsx");*/

       /* Excel.Application ex = new Microsoft.Office.Interop.Excel.Application();
        //Отобразить Excel
        ex.Visible = true;
        //Количество листов в рабочей книге
        ex.SheetsInNewWorkbook = 2;
        //Добавить рабочую книгу
        Excel.Workbook workBook = ex.Workbooks.Add(Type.Missing);
        //Отключить отображение окон с сообщениями
        ex.DisplayAlerts = false;                                       
        //Получаем первый лист документа (счет начинается с 1)
        Excel.Worksheet sheet = (Excel.Worksheet)ex.Worksheets.get_Item(1);
        //Название листа (вкладки снизу)
        sheet.Name = "Отчет за 13.12.2017";
        ex.Application.ActiveWorkbook.SaveAs("doc.xlsx", Type.Missing,
  Type.Missing, Type.Missing, Type.Missing, Type.Missing, Excel.XlSaveAsAccessMode.xlNoChange,
  Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);*/

    }
    public IEnumerator ReturnColor(Transform myTransform)
    {
        myTransform.GetComponent<Image>().color=  new Color(255.0f,0/255.0f,0/255.0f);   
        yield return new WaitForSeconds(0.2f);
        myTransform.GetComponent<Image>().color=  new Color(255.0f,255.0f,255.0f); 
        yield return new WaitForSeconds(0.2f);
        myTransform.GetComponent<Image>().color=  new Color(255.0f,0/255.0f,0/255.0f); 
        yield return new WaitForSeconds(0.2f);
        myTransform.GetComponent<Image>().color=  new Color(255.0f,255.0f,255.0f); 
        yield return new WaitForSeconds(0.2f);
        myTransform.GetComponent<Image>().color=  new Color(255.0f,0/255.0f,0/255.0f); 
        yield return new WaitForSeconds(0.2f);
        myTransform.GetComponent<Image>().color=  new Color(255.0f,255.0f,255.0f); 
    }
}
