using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;//позволяет обращаться и создавать  тектовые документы
using System;
using UnityEngine.EventSystems;
using UnityEngine.Video;




[System.Serializable]
public class Items
{
    
    public Transform alpfaform;//ссылка объект в контрагенте  
    public Transform Polatrans;//ссыка объект в рейсе

    public string Adress;
    public string telephone ;

    public int darina=0;//розничная цена
    public int darinaSum=0;//кло во дарины
    public int darinaZakup=0;//закупочная цена

    public int GornoyKrest=0;//розничная цена  
    public int GornoyKrestSum=0;//кло во 
    public int GornoyKrestZakup=0;//закупочная цена

    public int Dombay=0;//розничная цена
    public int DombaySum=0;//кло во домбая
    public int DombayZakup=0;//закупочная цена

    public int GornoyVersh=0;//розничная цена
    public int GornoyVershSum=0;//цена горной вершины
    public int GornoyVershZakup=0;//закупочная цена

    public int Pilegrim=0;//розничная цена
    public int PilegrimSum=0;//кло во 
    public int PilegrimZakup=0;//закупочная цена

    public int Kubay=0;//розничная цена
    public int KubaySum=0;//кло во 
    public int KubayZakup=0;//закупочная цена

    public int Tanais=0;//розничная цена
    public int TanaisSum=0;//кло во
    public int TanaisZakup=0;//закупочная цена
    
    public int index=0;
    public int WorkPrise=0;//зп водителя

    public bool isDebts=false;
    public bool isAlien=false;
    public bool edidObjekt=true;//спасобность объекта в маршрутнике быть редактируемым

}

[System.Serializable]
public class Debts  //долги
{
    public Transform Polatrans;//ссыка объект в долгах
    public string Adress;
    public string date;
    public int debt=0;//сумма долга
    public bool red; //это идентификатор цвета кнопки долга
}

public class NewBehaviourScript : MyClass
{
    public string data;
    public calendar calendar;

    //public Transform aaa;
    [Header("Экземпляры Кнопок")]
    public Transform PrefabKontragent;//префаб который вверху
    public Transform PrefabNomenklatura;
    public Transform PrefabNomenReis;
    public Transform PrefabDebtor;

    int cassa=0;
    int sumTara=0;
    int sumTaraVithet=0;
    
    int IndeXCriatePrefab = 0;//номер для полей контрагентов
    public int reis1Index=0;//номер для полей рейса №1
    public int reis2Index=0;//номер для полей рейса №2
    public int debtorIndex=0;//номер для полей долгов

    public int IndeXCriateNomenklatura = 0;//номер для полей номенклатуры
    int index = 0;//индекс для контрагентов
    public int Vidindex = 0;//индекс для видео
    public int kolDat=0;//кол-во дней с начала работы вообще
   
    public string acse = "";
   

    [Space]
    [Header("Массивы")]
    public List<Items> Kontragent = new List<Items>();
    public List<Items> KontragentSerch = new List<Items>();
    public List<Items> polaReis = new List<Items>();
    public List<Items> polaReis2 = new List<Items>();
    public List<Debts> debtor = new List<Debts>();
    
    public GameObject[] OptionNomen;//список ссылок на опции номенклатуры
   
    [Space]
    [Header("Установки Видео")]
    public VideoPlayer Vidplayer;
    public VideoClip[] VidClip;    

    [Space]
    [Header("Установки Цвета")]
    public FlexibleColorPicker fcp;
    public GameObject[] MASObjectColor;

    [Space]
    [Header("настройки")]
    public Font font;
    public Sprite ButtonSpritee;//для кнопки
    public Sprite InputFieldSpritee;//для поля ввода
    public GameObject by;//шаблон для создания новой кнопки
    public Transform reis1; 
    public Transform reis2; 
    Transform EnabledWindow;
    [Space]
    [Header("настройки поисковых панелей")]
    bool ok = true;
    bool isLoad=false;//переменная нужна для разрешения изменений полей масивов,выполняется после загрузки

    public Transform SerchPanel;//експеременты с отсеевателем
    public Transform SerchMenu;
    public Transform CopySerchMenu;
    public Transform ContentNomeklatura;
    public Transform scrollRect;
    public int IndexOtseevatel;//индекс для отсеевателя
    string color;
    public Color ColorAnton;

    void Start()
    {        
        data=System.DateTime.Now.ToString("d.MM.yyyy");
        // Massive.Add(default(Items));
        EnabledWindow = transform.parent.Find("Меню Настроек").transform;
        Load();
        //MASObjectColor[2].gameObject.SetActive(false);
    }
    
     void Update()
     {
        if (EnabledWindow.gameObject.activeInHierarchy)
        {
           foreach (GameObject i in MASObjectColor)
           {
               i.GetComponent<Image>().color = fcp.color;             
              
           }
           
        }
       // fcp.hexInput.text=color;//изменяет цвет окон исходя из сохранений
     }

    public void serch()//системма поиска
    {
        string ser = SerchPanel.GetComponent<InputField>().text;//сылка поискавой панельи
        if (ser == "")//если панель пуста то
        {
            SerchMenu.gameObject.SetActive(true);//основной массив включаем
            CopySerchMenu.gameObject.SetActive(false);//вспомогательный выключаем

        }
        else//если не пуста то
        {
            SerchMenu.gameObject.SetActive(false);//основной выключаем
            CopySerchMenu.gameObject.SetActive(true);//вспомогательный включаем
        }       
        if(ser != "")
        {
            for (int i=0;i<Kontragent.Count;i++)
            {
                if (i == 0)//каждую нулевую итерацию 
                {
                    for (int del = 0; del < KontragentSerch.Count; del++)//
                    {
                        KontragentSerch[del].alpfaform.Find("InputFieldName").GetComponent<InputField>().text = "";//убераем записи из копии масива контрагентов
                        KontragentSerch[del].alpfaform.Find("InputFieldTele").GetComponent<InputField>().text = "";//убераем записи из копии масива контрагентов
                    }

                    IndexOtseevatel = 0;
                }

                string kontrName = Kontragent[i].alpfaform.Find("InputFieldName").GetComponent<InputField>().text;//элемент массива   
                string kontrTele = Kontragent[i].alpfaform.Find("InputFieldTele").GetComponent<InputField>().text;//элемент массива   
                
                if (kontrName.ToLower().Contains(ser) && IndexOtseevatel < KontragentSerch.Count)//берем все записи,находим заглавные буквы и делаем их маленькими 
                                                //затем находим совпадения в словосочетаниях 
                {
                    ok = true;//это доступ
                    for (int x = 0; x < KontragentSerch.Count; x++)
                    {
                        if (KontragentSerch[IndexOtseevatel].alpfaform.Find("InputFieldName").GetComponent<InputField>().text == kontrName)
                        {
                            ok = false;
                        }
                    }
                
                    if (ok == true)
                    {
                        KontragentSerch[IndexOtseevatel].alpfaform.Find("InputFieldName").GetComponent<InputField>().text = kontrName;
                        KontragentSerch[IndexOtseevatel].alpfaform.Find("InputFieldTele").GetComponent<InputField>().text = kontrTele;
                        KontragentSerch[IndexOtseevatel].alpfaform.Find("Text").GetComponent<Text>().text = i.ToString();
                    }                            
                    IndexOtseevatel++;               
                }                 
            }  
        }             
    }   

    public void NextBakcground()//переключение заднего фона
    {

        if (Vidindex < VidClip.Length -1)
        {
           
            Vidindex++;
        }
        else Vidindex = 0;
                    
        Vidplayer.clip = VidClip[Vidindex];
         
    }

    public void CriatePrefab(Transform MyTransform)//префаб поля контрагента
    {      
        Transform Pref;
        Pref=Instantiate(PrefabKontragent);
        Pref.GetChild(0).GetComponent<Text>().text= IndeXCriatePrefab.ToString();//копируем порядочный номер в игровой объект
        IndeXCriatePrefab++;//порядочный номер

        Pref.name = "Кнопка";
        Pref.SetParent(MyTransform);     

        Kontragent.Add(new Items());   
        Kontragent[index].alpfaform = Pref.transform;
        Kontragent[index].index = index;

        //-----------устанавливаем стандартные цены------------//надо переделать в будуещем так что бы стандарт цен устанавливался из номенклатуры
        Kontragent[index].darinaZakup = 30;
        Kontragent[index].GornoyKrestZakup = 70;
        Kontragent[index].DombayZakup = 70;
        Kontragent[index].GornoyVershZakup = 130;
        Kontragent[index].PilegrimZakup = 135;
        Kontragent[index].KubayZakup = 135;
        Kontragent[index].TanaisZakup = 135;
        Kontragent[index].WorkPrise = 22;
        index++;
        
       
        StartCoroutine(routine: ScrollRect());       
    }

    public void CriateNomenklatura()//префаб поля номенклатуры
    {
        
        IndeXCriateNomenklatura++;
        Transform Pref;
        Pref = Instantiate(PrefabNomenklatura);
        Pref.GetChild(0).GetComponent<Text>().text = IndeXCriateNomenklatura.ToString();

        Pref.name = "Номенклатура";        
        Pref.SetParent(ContentNomeklatura.GetChild(0));

        CopiNomenINtipPrice(Pref);
    }

    public void CopiNomenINtipPrice(Transform Pref)//префаб поля номенклатуры копируется в "типы цен"
    {
        Pref = Instantiate(PrefabNomenklatura);
        Pref.GetChild(0).GetComponent<Text>().text = IndeXCriateNomenklatura.ToString();
        Pref.SetParent(transform.parent.Find("Меню Типа цен/Scrol/Content"));
        Pref.tag = "Nomen";
    }

    public void CopiNameKontragent(Transform MyTransform)//метод копирует инфу конторагента MyTransform это откуда копируется
    {       
         
        for (int i = 0; i < OptionNomen.Length; i++)
        {
            if (OptionNomen[i].transform.GetChild(0).GetComponent<Text>().text == MyTransform.parent.GetChild(0).GetComponent<Text>().text)//условие для копирования текста именно тойму полю с которым совпадает номер
            {
               OptionNomen[i].transform.GetChild(1).GetComponent<InputField>().text = MyTransform.GetComponent<InputField>().text;
            }            
        }
    }

    public void FindKontragent()//(Nomen)метод находит контрагента по тагу и вносит в массив для дальнейших копирований инфы
    {
        OptionNomen = GameObject.FindGameObjectsWithTag("Nomen");//сначало нахдим всех с тагом номен и вносим в простой массив
    }   

    public void FindKontagentAndDestroy(Transform MyTransform)
    {
        for (int i = 0; i < Kontragent.Count; i++)
        {
            if(MyTransform.GetComponent<Text>().text == Kontragent[i].Adress)
            {                
                destroy(Kontragent[i].alpfaform.gameObject); 
            }
        }
    }

    public void CopiPriceTipIdentidicater(Transform MyTransform)//передает идантефикатор типу цен для того что бы понять куда устанавливать цены также передает данные из Kontragent[] в ТИП цен
    {
        transform.parent.Find("Меню Типа цен/Identidicater").GetComponent<Text>().text=MyTransform.GetChild(1).GetComponent<InputField>().text;     
       
        transform.parent.Find("Меню Типа цен/Безнал").GetComponent<Toggle>().isOn = Kontragent[int.Parse(MyTransform.GetChild(0).GetComponent<Text>().text)].isDebts;        
        transform.parent.Find("Меню Типа цен/Чужой").GetComponent<Toggle>().isOn  = Kontragent[int.Parse(MyTransform.GetChild(0).GetComponent<Text>().text)].isAlien;        
        
        for(int i=0;i<OptionNomen.Length;i++)
        {
            if(OptionNomen[i].transform.GetChild(0).GetComponent<Text>().text=="0")
            {              
               OptionNomen[i].transform.GetChild(2).GetComponent<InputField>().text=
               Kontragent[int.Parse(MyTransform.GetChild(0).GetComponent<Text>().text)].darina.ToString();//ставим прайс розница

               OptionNomen[i].transform.GetChild(3).GetComponent<InputField>().text=
               Kontragent[int.Parse(MyTransform.GetChild(0).GetComponent<Text>().text)].darinaZakup.ToString();//ставим прайс закуп               
            }

            if(OptionNomen[i].transform.GetChild(0).GetComponent<Text>().text=="1")
            {              
               OptionNomen[i].transform.GetChild(2).GetComponent<InputField>().text=
               Kontragent[int.Parse(MyTransform.GetChild(0).GetComponent<Text>().text)].GornoyKrest.ToString();

               OptionNomen[i].transform.GetChild(3).GetComponent<InputField>().text=
               Kontragent[int.Parse(MyTransform.GetChild(0).GetComponent<Text>().text)].GornoyKrestZakup.ToString();
            }

            if(OptionNomen[i].transform.GetChild(0).GetComponent<Text>().text=="2")
            {              
               OptionNomen[i].transform.GetChild(2).GetComponent<InputField>().text=
               Kontragent[int.Parse(MyTransform.GetChild(0).GetComponent<Text>().text)].Dombay.ToString();

               OptionNomen[i].transform.GetChild(3).GetComponent<InputField>().text=
               Kontragent[int.Parse(MyTransform.GetChild(0).GetComponent<Text>().text)].DombayZakup.ToString();
            }

            if(OptionNomen[i].transform.GetChild(0).GetComponent<Text>().text=="3")
            {              
               OptionNomen[i].transform.GetChild(2).GetComponent<InputField>().text=
               Kontragent[int.Parse(MyTransform.GetChild(0).GetComponent<Text>().text)].GornoyVersh.ToString();

               OptionNomen[i].transform.GetChild(3).GetComponent<InputField>().text=
               Kontragent[int.Parse(MyTransform.GetChild(0).GetComponent<Text>().text)].GornoyVershZakup.ToString();
            }

            if(OptionNomen[i].transform.GetChild(0).GetComponent<Text>().text=="4")
            {              
               OptionNomen[i].transform.GetChild(2).GetComponent<InputField>().text=
               Kontragent[int.Parse(MyTransform.GetChild(0).GetComponent<Text>().text)].Pilegrim.ToString();

               OptionNomen[i].transform.GetChild(3).GetComponent<InputField>().text=
               Kontragent[int.Parse(MyTransform.GetChild(0).GetComponent<Text>().text)].PilegrimZakup.ToString();
            }

            if(OptionNomen[i].transform.GetChild(0).GetComponent<Text>().text=="5")
            {              
               OptionNomen[i].transform.GetChild(2).GetComponent<InputField>().text=
               Kontragent[int.Parse(MyTransform.GetChild(0).GetComponent<Text>().text)].Kubay.ToString();

               OptionNomen[i].transform.GetChild(3).GetComponent<InputField>().text=
               Kontragent[int.Parse(MyTransform.GetChild(0).GetComponent<Text>().text)].KubayZakup.ToString();
            }

            if(OptionNomen[i].transform.GetChild(0).GetComponent<Text>().text=="6")
            {              
               OptionNomen[i].transform.GetChild(2).GetComponent<InputField>().text=
               Kontragent[int.Parse(MyTransform.GetChild(0).GetComponent<Text>().text)].Tanais.ToString();

               OptionNomen[i].transform.GetChild(3).GetComponent<InputField>().text=
               Kontragent[int.Parse(MyTransform.GetChild(0).GetComponent<Text>().text)].TanaisZakup.ToString();
            }

            //конструкция наже странноватая но доступ иначе получить не получается
            //мы таким образом вставляем в типы цен ЗП водителя из ядра(Kontragent)
            OptionNomen[0].transform.parent.parent.parent.GetChild(5).GetComponent<InputField>().text=
            Kontragent[int.Parse(MyTransform.GetChild(0).GetComponent<Text>().text)].WorkPrise.ToString(); 
          
        }       
    }

    public void CopiPolaReisKolVo(Transform MyTransform)//передает в массив polaReis кол-во едениц 
    {      
        if(MyTransform.GetChild(3).GetComponent<InputField>().text != "" && MyTransform.GetChild(3).GetComponent<InputField>().text !="-")
        {
            for (int i = 0; i < polaReis.Count; i++)
            {
               if(polaReis[i].Polatrans.gameObject == MyTransform.gameObject)
               {
                   polaReis[ int.Parse(MyTransform.GetChild(0).GetComponent<Text>().text)].darinaSum =
                   int.Parse(MyTransform.GetChild(3).GetComponent<InputField>().text);
               }                
            }

            for (int i = 0; i < polaReis2.Count; i++)
            {
               if(polaReis2[i].Polatrans.gameObject == MyTransform.gameObject)
               {
                   polaReis2[ int.Parse(MyTransform.GetChild(0).GetComponent<Text>().text)].darinaSum =
                   int.Parse(MyTransform.GetChild(3).GetComponent<InputField>().text);
               }                
            }
        }
        else
        {
            for (int i = 0; i < polaReis.Count; i++)
            {
                if(polaReis[i].Polatrans.gameObject == MyTransform.gameObject)
               {
                   polaReis[int.Parse(MyTransform.GetChild(0).GetComponent<Text>().text)].darinaSum = 0;
                   MyTransform.GetChild(3).GetComponent<InputField>().text = "";
               }
            }
            for (int i = 0; i < polaReis2.Count; i++)
            {
               if(polaReis2[i].Polatrans.gameObject==MyTransform.gameObject)
               {
                   polaReis2[int.Parse(MyTransform.GetChild(0).GetComponent<Text>().text)].darinaSum = 0;
                   MyTransform.GetChild(3).GetComponent<InputField>().text = "";
               }
            }            
        }
        
       if(MyTransform.GetChild(4).GetComponent<InputField>().text != "" && MyTransform.GetChild(4).GetComponent<InputField>().text != "-")
        {
            for (int i = 0; i < polaReis.Count; i++)
            {
               if(polaReis[i].Polatrans.gameObject==MyTransform.gameObject)
               {
                    polaReis[ int.Parse(MyTransform.GetChild(0).GetComponent<Text>().text)].GornoyKrestSum=
                    int.Parse(MyTransform.GetChild(4).GetComponent<InputField>().text);
               }                
            }

            for (int i = 0; i < polaReis2.Count; i++)
            {
               if(polaReis2[i].Polatrans.gameObject==MyTransform.gameObject)
               {
                    polaReis2[ int.Parse(MyTransform.GetChild(0).GetComponent<Text>().text)].GornoyKrestSum=
                    int.Parse(MyTransform.GetChild(4).GetComponent<InputField>().text);
               }                
            }           
        }
        else
        {
            for (int i = 0; i < polaReis.Count; i++)
            {
                if(polaReis[i].Polatrans.gameObject==MyTransform.gameObject)
               {
                    polaReis[ int.Parse(MyTransform.GetChild(0).GetComponent<Text>().text)].GornoyKrestSum=0;
                    MyTransform.GetChild(4).GetComponent<InputField>().text = "";
               }
            }
            for (int i = 0; i < polaReis2.Count; i++)
            {
               if(polaReis2[i].Polatrans.gameObject==MyTransform.gameObject)
               {
                    polaReis2[ int.Parse(MyTransform.GetChild(0).GetComponent<Text>().text)].GornoyKrestSum=0;
                    MyTransform.GetChild(4).GetComponent<InputField>().text = "";
               }
            }           
        }        

        if(MyTransform.GetChild(5).GetComponent<InputField>().text != "" && MyTransform.GetChild(5).GetComponent<InputField>().text!="-")
        {
            for (int i = 0; i < polaReis.Count; i++)
            {
               if(polaReis[i].Polatrans.gameObject==MyTransform.gameObject)
               {
                    polaReis[ int.Parse(MyTransform.GetChild(0).GetComponent<Text>().text)].DombaySum=
                    int.Parse(MyTransform.GetChild(5).GetComponent<InputField>().text);
               }                
            }

            for (int i = 0; i < polaReis2.Count; i++)
            {
               if(polaReis2[i].Polatrans.gameObject==MyTransform.gameObject)
               {
                    polaReis2[ int.Parse(MyTransform.GetChild(0).GetComponent<Text>().text)].DombaySum=
                    int.Parse(MyTransform.GetChild(5).GetComponent<InputField>().text);
               }                
            }            
        }
        else
        {
            for (int i = 0; i < polaReis.Count; i++)
            {
                if(polaReis[i].Polatrans.gameObject==MyTransform.gameObject)
               {
                    polaReis[ int.Parse(MyTransform.GetChild(0).GetComponent<Text>().text)].DombaySum=0;
                    MyTransform.GetChild(5).GetComponent<InputField>().text="";
               }
            }
            for (int i = 0; i < polaReis2.Count; i++)
            {
               if(polaReis2[i].Polatrans.gameObject==MyTransform.gameObject)
               {
                    polaReis2[ int.Parse(MyTransform.GetChild(0).GetComponent<Text>().text)].DombaySum=0;
                    MyTransform.GetChild(5).GetComponent<InputField>().text="";
               }
            }            
        }
        
        if(MyTransform.GetChild(6).GetComponent<InputField>().text != "" && MyTransform.GetChild(6).GetComponent<InputField>().text!="-")
        {
            for (int i = 0; i < polaReis.Count; i++)
            {
               if(polaReis[i].Polatrans.gameObject==MyTransform.gameObject)
               {
                    polaReis[ int.Parse(MyTransform.GetChild(0).GetComponent<Text>().text)].GornoyVershSum=
                    int.Parse(MyTransform.GetChild(6).GetComponent<InputField>().text);
               }                
            }

            for (int i = 0; i < polaReis2.Count; i++)
            {
               if(polaReis2[i].Polatrans.gameObject==MyTransform.gameObject)
               {
                    polaReis2[ int.Parse(MyTransform.GetChild(0).GetComponent<Text>().text)].GornoyVershSum=
                    int.Parse(MyTransform.GetChild(6).GetComponent<InputField>().text);
               }                
            }            
        }
        else
        {
            for (int i = 0; i < polaReis.Count; i++)
            {
                if(polaReis[i].Polatrans.gameObject==MyTransform.gameObject)
               {
                    polaReis[ int.Parse(MyTransform.GetChild(0).GetComponent<Text>().text)].GornoyVershSum=0;
                    MyTransform.GetChild(6).GetComponent<InputField>().text="";
               }
            }
            for (int i = 0; i < polaReis2.Count; i++)
            {
               if(polaReis2[i].Polatrans.gameObject==MyTransform.gameObject)
               {
                    polaReis2[ int.Parse(MyTransform.GetChild(0).GetComponent<Text>().text)].GornoyVershSum=0;
                    MyTransform.GetChild(6).GetComponent<InputField>().text="";
               }
            }              
        }

       if(MyTransform.GetChild(7).GetComponent<InputField>().text != "" && MyTransform.GetChild(7).GetComponent<InputField>().text!="-")
        {
            for (int i = 0; i < polaReis.Count; i++)
            {
               if(polaReis[i].Polatrans.gameObject==MyTransform.gameObject)
               {
                    polaReis[ int.Parse(MyTransform.GetChild(0).GetComponent<Text>().text)].PilegrimSum=
                    int.Parse(MyTransform.GetChild(7).GetComponent<InputField>().text);
               }                
            }

            for (int i = 0; i < polaReis2.Count; i++)
            {
               if(polaReis2[i].Polatrans.gameObject==MyTransform.gameObject)
               {
                    polaReis2[ int.Parse(MyTransform.GetChild(0).GetComponent<Text>().text)].PilegrimSum=
                    int.Parse(MyTransform.GetChild(7).GetComponent<InputField>().text);
               }                
            }            
        }
        else
        {
            for (int i = 0; i < polaReis.Count; i++)
            {
                if(polaReis[i].Polatrans.gameObject==MyTransform.gameObject)
               {
                    polaReis[ int.Parse(MyTransform.GetChild(0).GetComponent<Text>().text)].PilegrimSum=0;
                    MyTransform.GetChild(7).GetComponent<InputField>().text="";
               }
            }
            for (int i = 0; i < polaReis2.Count; i++)
            {
               if(polaReis2[i].Polatrans.gameObject==MyTransform.gameObject)
               {
                    polaReis2[ int.Parse(MyTransform.GetChild(0).GetComponent<Text>().text)].PilegrimSum=0;
                    MyTransform.GetChild(7).GetComponent<InputField>().text="";
               }
            }            
        }

       if(MyTransform.GetChild(8).GetComponent<InputField>().text != "" && MyTransform.GetChild(8).GetComponent<InputField>().text!="-")
        {
            for (int i = 0; i < polaReis.Count; i++)
            {
               if(polaReis[i].Polatrans.gameObject==MyTransform.gameObject)
               {
                    polaReis[ int.Parse(MyTransform.GetChild(0).GetComponent<Text>().text)].KubaySum=
                    int.Parse(MyTransform.GetChild(8).GetComponent<InputField>().text);
               }                
            }

            for (int i = 0; i < polaReis2.Count; i++)
            {
               if(polaReis2[i].Polatrans.gameObject==MyTransform.gameObject)
               {
                    polaReis2[ int.Parse(MyTransform.GetChild(0).GetComponent<Text>().text)].KubaySum=
                    int.Parse(MyTransform.GetChild(8).GetComponent<InputField>().text);
               }                
            }            
        }
        else
        {
            for (int i = 0; i < polaReis.Count; i++)
            {
                if(polaReis[i].Polatrans.gameObject==MyTransform.gameObject)
               {
                    polaReis[ int.Parse(MyTransform.GetChild(0).GetComponent<Text>().text)].KubaySum=0;
                    MyTransform.GetChild(8).GetComponent<InputField>().text="";
               }
            }
            for (int i = 0; i < polaReis2.Count; i++)
            {
               if(polaReis2[i].Polatrans.gameObject==MyTransform.gameObject)
               {
                    polaReis2[ int.Parse(MyTransform.GetChild(0).GetComponent<Text>().text)].KubaySum=0;
                    MyTransform.GetChild(8).GetComponent<InputField>().text="";
               }
            }            
        }

       if(MyTransform.GetChild(9).GetComponent<InputField>().text != "" && MyTransform.GetChild(9).GetComponent<InputField>().text!="-")
        {
            for (int i = 0; i < polaReis.Count; i++)
            {
               if(polaReis[i].Polatrans.gameObject==MyTransform.gameObject)
               {
                    polaReis[ int.Parse(MyTransform.GetChild(0).GetComponent<Text>().text)].TanaisSum=
                    int.Parse(MyTransform.GetChild(9).GetComponent<InputField>().text);
               }                
            }

            for (int i = 0; i < polaReis2.Count; i++)
            {
               if(polaReis2[i].Polatrans.gameObject==MyTransform.gameObject)
               {
                    polaReis2[ int.Parse(MyTransform.GetChild(0).GetComponent<Text>().text)].TanaisSum=
                    int.Parse(MyTransform.GetChild(9).GetComponent<InputField>().text);
               }                
            }             
        }
        else
        {
            for (int i = 0; i < polaReis.Count; i++)
            {
                if(polaReis[i].Polatrans.gameObject==MyTransform.gameObject)
               {
                    polaReis[ int.Parse(MyTransform.GetChild(0).GetComponent<Text>().text)].TanaisSum=0;
                    MyTransform.GetChild(9).GetComponent<InputField>().text="";
               }
            }
            for (int i = 0; i < polaReis2.Count; i++)
            {
               if(polaReis2[i].Polatrans.gameObject==MyTransform.gameObject)
               {
                    polaReis2[ int.Parse(MyTransform.GetChild(0).GetComponent<Text>().text)].TanaisSum=0;
                    MyTransform.GetChild(9).GetComponent<InputField>().text="";
               }
            }             
        }     
    }

    public void CopiPriceInfo(Transform MyTransform)//переносит из ТИП ЦЕН розничную цену,закупачную цену и ЗП водителя в главный массив(Kontragent[])
    {   
        if(MyTransform.GetComponent<InputField>().text == "" || MyTransform.GetComponent<InputField>().text == "-")
        {
            MyTransform.GetComponent<InputField>().text = "0";
        }
        if(MyTransform.name!="ЗП водителя")//условие нужно для передачи цен а не ЗП...зп передается отдельно от этой конструкции
        {
            for(int i = 0;i<Kontragent.Count;i++)
            {           
                if(transform.parent.Find("Меню Типа цен/Identidicater").GetComponent<Text>().text==Kontragent[i].Adress)
                {

                    if(MyTransform.parent.GetChild(0).GetComponent<Text>().text=="0")
                    {
                        if(MyTransform.name == "Price")
                        {
                            Kontragent[i].darina=int.Parse(MyTransform.GetComponent<InputField>().text);
                        }
                        else if(MyTransform.name == "PriceZakup")
                        {
                            Kontragent[i].darinaZakup=int.Parse(MyTransform.GetComponent<InputField>().text);
                        }
                        
                    }

                    if(MyTransform.parent.GetChild(0).GetComponent<Text>().text=="1")
                    {
                        if(MyTransform.name == "Price")
                        {
                            Kontragent[i].GornoyKrest=int.Parse(MyTransform.GetComponent<InputField>().text);
                        }
                        else if(MyTransform.name == "PriceZakup")
                        {
                            Kontragent[i].GornoyKrestZakup=int.Parse(MyTransform.GetComponent<InputField>().text);
                        }
                    }

                    if(MyTransform.parent.GetChild(0).GetComponent<Text>().text=="2")
                    {
                        if(MyTransform.name == "Price")
                        {
                            Kontragent[i].Dombay=int.Parse(MyTransform.GetComponent<InputField>().text);
                        }
                        else if(MyTransform.name == "PriceZakup")
                        {
                            Kontragent[i].DombayZakup=int.Parse(MyTransform.GetComponent<InputField>().text);
                        }
                    }

                    if(MyTransform.parent.GetChild(0).GetComponent<Text>().text=="3")
                    {
                        if(MyTransform.name == "Price")
                        {
                            Kontragent[i].GornoyVersh=int.Parse(MyTransform.GetComponent<InputField>().text);
                        }
                        else if(MyTransform.name == "PriceZakup")
                        {
                            Kontragent[i].GornoyVershZakup=int.Parse(MyTransform.GetComponent<InputField>().text);
                        }
                    }

                    if(MyTransform.parent.GetChild(0).GetComponent<Text>().text=="4")
                    {
                        if(MyTransform.name == "Price")
                        {
                            Kontragent[i].Pilegrim=int.Parse(MyTransform.GetComponent<InputField>().text);
                        }
                        else if(MyTransform.name == "PriceZakup")
                        {
                            Kontragent[i].PilegrimZakup=int.Parse(MyTransform.GetComponent<InputField>().text);
                        }
                    }

                    if(MyTransform.parent.GetChild(0).GetComponent<Text>().text=="5")
                    {
                        if(MyTransform.name == "Price")
                        {
                            Kontragent[i].Kubay=int.Parse(MyTransform.GetComponent<InputField>().text);
                        }
                        else if(MyTransform.name == "PriceZakup")
                        {
                            Kontragent[i].KubayZakup=int.Parse(MyTransform.GetComponent<InputField>().text);
                        }
                    }

                    if(MyTransform.parent.GetChild(0).GetComponent<Text>().text=="6")
                    {
                        if(MyTransform.name == "Price")
                        {
                            Kontragent[i].Tanais=int.Parse(MyTransform.GetComponent<InputField>().text);
                        }
                        else if(MyTransform.name == "PriceZakup")
                        {
                            Kontragent[i].TanaisZakup=int.Parse(MyTransform.GetComponent<InputField>().text);
                        }
                    }
                }
            }
        } 
        else
        {
            for(int i=0;i<Kontragent.Count;i++)
            {           
                if(transform.parent.Find("Меню Типа цен/Identidicater").GetComponent<Text>().text==Kontragent[i].Adress)
                {
                    Kontragent[i].WorkPrise=int.Parse(MyTransform.GetComponent<InputField>().text);                    
                }
            }
        }
    }
    
    public void kontrKopiMassive(Transform MyTransform) 
    {      
      if(isLoad)
      {
          Kontragent[ int.Parse(MyTransform.GetChild(0).GetComponent<Text>().text)].Adress=MyTransform.GetChild(1).GetComponent<InputField>().text;
           
          Kontragent[ int.Parse(MyTransform.GetChild(0).GetComponent<Text>().text)].telephone=MyTransform.GetChild(2).GetComponent<InputField>().text;
      }       
    }

    public void Podschet()//суммирует! умножает кол-во на цену в рейсе ,так же выводит прибыль и зарплаты 
    {        
        int Sum=0;       //выручка аодреса
        int SumVir=0;   //сумма выручки для подчсчета прибыли
        int SumBeznal =0;   //сумма выручки для подчсчета прибыли безнала
        int DarinaSum=0;    //сумма бутылок дарина
        int GornKresSum=0;  //сумма бутылок гресталл
        int DombaySum=0;  //сумма бутылок гресталл
        int GornayaSum=0;  //сумма бутылок гресталл
        int PilSum=0;  //сумма бутылок гресталл
        int KubSum=0;  //сумма бутылок гресталл
        int TanSum=0;  //сумма бутылок гресталл
        int ObjKolSum=0;    //общая сумма всех бутылок 
        int ZPDriver=0; //сумма зп водителя       
        int pribl=0; // прибыль
        int Zakupka=0; //закупка
        int ZakupkaBezDarina=0; //закупка без дарины        
        int alianMinus = 0; // если контрагент чужой то это поле берет его выручку и вычитает ее из общей прибыли
        //-------------------------------------- подчсет для рейса 1 --------------------------------------------//
        for(int i=0;i<polaReis.Count;i++)
        {   
            for (int y = 0; y < 7; y++)
            {
                
                if(polaReis[i].Polatrans.GetChild(3+y).Find("Text").GetComponent<Text>().text != "")
                {
                    polaReis[i].Polatrans.GetChild(3+y).GetComponent<Image>().color = new Color(255.0f,255.0f,255.0f);
                } 
                else
                {
                    polaReis[i].Polatrans.GetChild(3+y).GetComponent<Image>().color = new Color(115/255.0f,115/255.0f,115/255.0f);
                }   
            }
            

            Sum=polaReis[i].darina*polaReis[i].darinaSum+
            polaReis[i].GornoyKrest*polaReis[i].GornoyKrestSum+
            polaReis[i].Dombay*polaReis[i].DombaySum+
            polaReis[i].GornoyVersh*polaReis[i].GornoyVershSum+
            polaReis[i].Pilegrim*polaReis[i].PilegrimSum+
            polaReis[i].Kubay*polaReis[i].KubaySum+
            polaReis[i].Tanais*polaReis[i].TanaisSum;
            
            polaReis[i].Polatrans.GetChild(10).GetComponent<InputField>().text =Sum.ToString();

            if(polaReis[i].isDebts && !polaReis[i].isAlien)// если безнал
            {
                // Debug.Log("клиент безнальный");
                //  polaReis[i].Polatrans.GetChild(10).GetComponent<InputField>().text ="Без";
                SumBeznal += Sum;
                DarinaSum+=polaReis[i].darinaSum;
                GornKresSum+=polaReis[i].GornoyKrestSum;
                DombaySum+=polaReis[i].DombaySum;
                GornayaSum+=polaReis[i].GornoyVershSum;
                PilSum+=polaReis[i].PilegrimSum;
                KubSum+=polaReis[i].KubaySum;
                TanSum+=polaReis[i].TanaisSum;      

                ZPDriver += (polaReis[i].darinaSum + polaReis[i].GornoyKrestSum + polaReis[i].DombaySum 
                + polaReis[i].GornoyVershSum + polaReis[i].PilegrimSum + polaReis[i].KubaySum 
                + polaReis[i].TanaisSum) * polaReis[i].WorkPrise;

                Zakupka += (polaReis[i].darinaSum * polaReis[i].darinaZakup) + (polaReis[i].GornoyKrestSum * polaReis[i].GornoyKrestZakup)
                      + (polaReis[i].DombaySum * polaReis[i].DombayZakup) + (polaReis[i].GornoyVershSum * polaReis[i].GornoyVershZakup)
                      + (polaReis[i].PilegrimSum * polaReis[i].PilegrimZakup)
                      + (polaReis[i].KubaySum * polaReis[i].KubayZakup)
                      + (polaReis[i].TanaisSum * polaReis[i].TanaisZakup);              

                ZakupkaBezDarina += (polaReis[i].GornoyKrestSum * polaReis[i].GornoyKrestZakup)
                      + (polaReis[i].DombaySum * polaReis[i].DombayZakup) 
                      + (polaReis[i].GornoyVershSum * polaReis[i].GornoyVershZakup)
                      + (polaReis[i].PilegrimSum * polaReis[i].PilegrimZakup) 
                      + (polaReis[i].KubaySum * polaReis[i].KubayZakup)
                      + (polaReis[i].TanaisSum * polaReis[i].TanaisZakup);
            }
            else 
            if(!polaReis[i].isDebts && polaReis[i].isAlien)// если чужой
            {
              //  polaReis[i].Polatrans.GetChild(10).GetComponent<InputField>().text =Sum.ToString();
                /* Zakupka +=  (polaReis[i].darinaSum * polaReis[i].darinaZakup) + (polaReis[i].GornoyKrestSum * polaReis[i].GornoyKrestZakup)
                      + (polaReis[i].DombaySum * polaReis[i].DombayZakup) + (polaReis[i].GornoyVershSum * polaReis[i].GornoyVershZakup)
                      + (polaReis[i].PilegrimSum * polaReis[i].PilegrimZakup) + (polaReis[i].KubaySum * polaReis[i].KubayZakup);*/ 
                 
               
                  
                DarinaSum+=polaReis[i].darinaSum;
                GornKresSum+=polaReis[i].GornoyKrestSum;
                DombaySum+=polaReis[i].DombaySum;
                GornayaSum+=polaReis[i].GornoyVershSum;
                PilSum+=polaReis[i].PilegrimSum;
                KubSum+=polaReis[i].KubaySum;
                TanSum+=polaReis[i].TanaisSum;      
                
                int alian=(polaReis[i].darinaSum + polaReis[i].GornoyKrestSum + polaReis[i].DombaySum 
                + polaReis[i].GornoyVershSum + polaReis[i].PilegrimSum + polaReis[i].KubaySum 
                + polaReis[i].TanaisSum) * polaReis[i].WorkPrise;

                ZPDriver += alian;
                SumVir += Sum;  

                alianMinus += (Sum - alian);       
               
            }
            else
            if(!polaReis[i].isDebts && !polaReis[i].isAlien)// если обычный
            {
                 
                SumVir += Sum; 
                DarinaSum+=polaReis[i].darinaSum;
                GornKresSum+=polaReis[i].GornoyKrestSum;
                DombaySum+=polaReis[i].DombaySum;
                GornayaSum+=polaReis[i].GornoyVershSum;
                PilSum+=polaReis[i].PilegrimSum;
                KubSum+=polaReis[i].KubaySum;
                TanSum+=polaReis[i].TanaisSum;      

                ZPDriver += (polaReis[i].darinaSum + polaReis[i].GornoyKrestSum + polaReis[i].DombaySum 
                + polaReis[i].GornoyVershSum + polaReis[i].PilegrimSum + polaReis[i].KubaySum 
                + polaReis[i].TanaisSum) * polaReis[i].WorkPrise;

                Zakupka +=  (polaReis[i].darinaSum * polaReis[i].darinaZakup) + (polaReis[i].GornoyKrestSum * polaReis[i].GornoyKrestZakup)
                      + (polaReis[i].DombaySum * polaReis[i].DombayZakup) + (polaReis[i].GornoyVershSum * polaReis[i].GornoyVershZakup)
                      + (polaReis[i].PilegrimSum * polaReis[i].PilegrimZakup) 
                      + (polaReis[i].KubaySum * polaReis[i].KubayZakup)
                      + (polaReis[i].TanaisSum * polaReis[i].TanaisZakup);

                ZakupkaBezDarina += (polaReis[i].GornoyKrestSum * polaReis[i].GornoyKrestZakup)
                      + (polaReis[i].DombaySum * polaReis[i].DombayZakup) + (polaReis[i].GornoyVershSum * polaReis[i].GornoyVershZakup)
                      + (polaReis[i].PilegrimSum * polaReis[i].PilegrimZakup) 
                      + (polaReis[i].KubaySum * polaReis[i].KubayZakup)
                      + (polaReis[i].TanaisSum * polaReis[i].TanaisZakup);
                
            }
        }
       // Debug.Log("алиан минус " + alianMinus);  
       // Debug.Log("сумма выр " + SumVir);  
       // Debug.Log("закупка " + Zakupka);  
       // Debug.Log("зп водителя " + ZPDriver);  
        pribl = (SumVir + SumBeznal) - (Zakupka + ZPDriver) - alianMinus;
        
        ObjKolSum=DarinaSum+GornKresSum+DombaySum+GornayaSum+PilSum+KubSum+TanSum;    

        reis1.GetChild(5).GetComponent<InputField>().text=SumVir.ToString();//сюда заносим общую сумму денег
        reis1.GetChild(8).GetComponent<InputField>().text=DarinaSum.ToString();
        reis1.GetChild(9).GetComponent<InputField>().text=GornKresSum.ToString();
        reis1.GetChild(10).GetComponent<InputField>().text=DombaySum.ToString();
        reis1.GetChild(11).GetComponent<InputField>().text=GornayaSum.ToString();
        reis1.GetChild(12).GetComponent<InputField>().text=PilSum.ToString();
        reis1.GetChild(13).GetComponent<InputField>().text=KubSum.ToString();
        reis1.GetChild(14).GetComponent<InputField>().text=TanSum.ToString();
        reis1.GetChild(15).GetComponent<InputField>().text=ObjKolSum.ToString();//сюда заносим общую сумму всех бутылок
        reis1.GetChild(7).GetComponent<InputField>().text=ZPDriver.ToString();//сюда заносим зп водителя
        reis1.GetChild(16).GetComponent<InputField>().text=Zakupka.ToString();
        reis1.GetChild(6).GetComponent<InputField>().text=pribl.ToString();
        reis1.GetChild(17).GetComponent<InputField>().text=ZakupkaBezDarina.ToString();
       // Debug.Log("пересчет ");   

        //-------------------------------------- подчсет для рейса 2 --------------------------------------------//
        Sum=0;       
        SumVir=0;   //сумма выручки
        DarinaSum=0;    //сумма бутылок дарина
        GornKresSum=0;  //сумма бутылок гресталл
        DombaySum=0;  //сумма бутылок гресталл
        GornayaSum=0;  //сумма бутылок гресталл
        PilSum=0;  //сумма бутылок гресталл
        KubSum=0;  //сумма бутылок гресталл
        TanSum=0;  //сумма бутылок гресталл
        ObjKolSum=0;    //общая сумма всех бутылок 
        ZPDriver=0; //сумма зп водителя       
        pribl=0; // прибыль
        Zakupka=0; //закупка
        ZakupkaBezDarina=0; //закупка без дарины  
        alianMinus=0;  
        for(int i=0;i<polaReis2.Count;i++)
        {   
            for (int y = 0; y < 7; y++)
            {
                
                if(polaReis2[i].Polatrans.GetChild(3+y).Find("Text").GetComponent<Text>().text != "")
                {
                    polaReis2[i].Polatrans.GetChild(3+y).GetComponent<Image>().color = new Color(255.0f,255.0f,255.0f);
                } 
                else
                {
                     polaReis2[i].Polatrans.GetChild(3+y).GetComponent<Image>().color = new Color(115/255.0f,115/255.0f,115/255.0f);
                }   
            }
            

            Sum=polaReis2[i].darina*polaReis2[i].darinaSum+
            polaReis2[i].GornoyKrest*polaReis2[i].GornoyKrestSum+
            polaReis2[i].Dombay*polaReis2[i].DombaySum+
            polaReis2[i].GornoyVersh*polaReis2[i].GornoyVershSum+
            polaReis2[i].Pilegrim*polaReis2[i].PilegrimSum+
            polaReis2[i].Kubay*polaReis2[i].KubaySum+
            polaReis2[i].Tanais*polaReis2[i].TanaisSum;

            polaReis2[i].Polatrans.GetChild(10).GetComponent<InputField>().text =Sum.ToString();

            if(polaReis2[i].isDebts && !polaReis2[i].isAlien)// если безнал
            {
                DarinaSum+=polaReis2[i].darinaSum;
                GornKresSum+=polaReis2[i].GornoyKrestSum;
                DombaySum+=polaReis2[i].DombaySum;
                GornayaSum+=polaReis2[i].GornoyVershSum;
                PilSum+=polaReis2[i].PilegrimSum;
                KubSum+=polaReis2[i].KubaySum;
                TanSum+=polaReis2[i].TanaisSum;          

                ZPDriver += (polaReis2[i].darinaSum + polaReis2[i].GornoyKrestSum + polaReis2[i].DombaySum 
                + polaReis2[i].GornoyVershSum + polaReis2[i].PilegrimSum + polaReis2[i].KubaySum 
                + polaReis2[i].TanaisSum) * polaReis2[i].WorkPrise;
            }
            else
            if(!polaReis2[i].isDebts && polaReis2[i].isAlien)// если чужой
            {
                
                DarinaSum+=polaReis2[i].darinaSum;
                GornKresSum+=polaReis2[i].GornoyKrestSum;
                DombaySum+=polaReis2[i].DombaySum;
                GornayaSum+=polaReis2[i].GornoyVershSum;
                PilSum+=polaReis2[i].PilegrimSum;
                KubSum+=polaReis2[i].KubaySum;
                TanSum+=polaReis2[i].TanaisSum;          

                int alian=(polaReis2[i].darinaSum + polaReis2[i].GornoyKrestSum + polaReis2[i].DombaySum 
                + polaReis2[i].GornoyVershSum + polaReis2[i].PilegrimSum + polaReis2[i].KubaySum 
                + polaReis2[i].TanaisSum) * polaReis2[i].WorkPrise;

                ZPDriver += alian;
                
                SumVir += Sum;
                alianMinus += (Sum - alian);       
            }
            else
            if(!polaReis2[i].isDebts && !polaReis2[i].isAlien)// если обычный
            {
                SumVir += Sum;
                DarinaSum+=polaReis2[i].darinaSum;
                GornKresSum+=polaReis2[i].GornoyKrestSum;
                DombaySum+=polaReis2[i].DombaySum;
                GornayaSum+=polaReis2[i].GornoyVershSum;
                PilSum+=polaReis2[i].PilegrimSum;
                KubSum+=polaReis2[i].KubaySum;
                TanSum+=polaReis2[i].TanaisSum;          

                ZPDriver += (polaReis2[i].darinaSum + polaReis2[i].GornoyKrestSum + polaReis2[i].DombaySum 
                + polaReis2[i].GornoyVershSum + polaReis2[i].PilegrimSum + polaReis2[i].KubaySum 
                + polaReis2[i].TanaisSum) * polaReis2[i].WorkPrise;

                Zakupka +=  (polaReis2[i].darinaSum * polaReis2[i].darinaZakup) + (polaReis2[i].GornoyKrestSum * polaReis2[i].GornoyKrestZakup)
                        + (polaReis2[i].DombaySum * polaReis2[i].DombayZakup) + (polaReis2[i].GornoyVershSum * polaReis2[i].GornoyVershZakup)
                        + (polaReis2[i].PilegrimSum * polaReis2[i].PilegrimZakup) 
                        + (polaReis2[i].KubaySum * polaReis2[i].KubayZakup)
                        + (polaReis2[i].TanaisSum * polaReis2[i].TanaisZakup);

                ZakupkaBezDarina += (polaReis2[i].GornoyKrestSum * polaReis2[i].GornoyKrestZakup)
                        + (polaReis2[i].DombaySum * polaReis2[i].DombayZakup) + (polaReis2[i].GornoyVershSum * polaReis2[i].GornoyVershZakup)
                        + (polaReis2[i].PilegrimSum * polaReis2[i].PilegrimZakup) 
                        + (polaReis2[i].KubaySum * polaReis2[i].KubayZakup)
                        + (polaReis2[i].TanaisSum * polaReis2[i].TanaisZakup);
            }
        }
        pribl = SumVir - (Zakupka + ZPDriver)-alianMinus;
        
        ObjKolSum=DarinaSum+GornKresSum+DombaySum+GornayaSum+PilSum+KubSum+TanSum;    

        reis2.GetChild(5).GetComponent<InputField>().text=SumVir.ToString();//сюда заносим общую сумму денег
        reis2.GetChild(8).GetComponent<InputField>().text=DarinaSum.ToString();
        reis2.GetChild(9).GetComponent<InputField>().text=GornKresSum.ToString();
        reis2.GetChild(10).GetComponent<InputField>().text=DombaySum.ToString();
        reis2.GetChild(11).GetComponent<InputField>().text=GornayaSum.ToString();
        reis2.GetChild(12).GetComponent<InputField>().text=PilSum.ToString();
        reis2.GetChild(13).GetComponent<InputField>().text=KubSum.ToString();
        reis2.GetChild(14).GetComponent<InputField>().text=TanSum.ToString();
        reis2.GetChild(15).GetComponent<InputField>().text=ObjKolSum.ToString();//сюда заносим общую сумму всех бутылок
        reis2.GetChild(7).GetComponent<InputField>().text=ZPDriver.ToString();//сюда заносим зп водителя
        reis2.GetChild(16).GetComponent<InputField>().text=Zakupka.ToString();
        reis2.GetChild(6).GetComponent<InputField>().text=pribl.ToString();
        reis2.GetChild(17).GetComponent<InputField>().text=ZakupkaBezDarina.ToString();
       // Debug.Log("пересчет ");   

        for (int i = 0; i < polaReis.Count; i++)
        {
            
            if(polaReis[i].isDebts || polaReis[i].isAlien)
            {
                AddDebts(polaReis[i]);                  
            }
        }
        for (int i = 0; i < polaReis2.Count; i++)
        {
            if(polaReis2[i].isDebts || polaReis2[i].isAlien)
            {
                AddDebts(polaReis2[i]);
            }
        }
    }
   
    public void CriateButton(Transform MyTransform)//Сотворить
    {          
          GameObject Button = new GameObject("Кнопка",typeof (Image), typeof(Button), typeof(LayoutElement));
          Button.transform.SetParent(MyTransform);
          Button.GetComponent<RectTransform>().sizeDelta = new Vector2(30, 30);
         // Button.GetComponent<RectTransform>().position = new Vector2(150, 280);
          Button.GetComponent<Image>().sprite = ButtonSpritee; //Resources.Load<Sprite>("Assets/UISprite");
          Button.GetComponent<Image>().type= by.GetComponent<Image>().type;
          Button.GetComponent<LayoutElement>().minHeight = 35;
       //   Button.GetComponent<Button>().onClick.AddListener(delegate { PressButton(); });
        
          GameObject TextButton = new GameObject("Text", typeof(Text));//текст кнопки
          TextButton.transform.SetParent(Button.transform);
          TextButton.GetComponent<RectTransform>().sizeDelta = new Vector2(30,30);
         // TextButton.GetComponent<RectTransform>().position = new Vector2(160, 30);
          TextButton.GetComponent<Text>().text = index.ToString();
          TextButton.GetComponent<Text>().font = font;
          TextButton.GetComponent<Text>().color = Color.black;
          TextButton.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;

          GameObject TextInputmenu = new GameObject("Text", typeof(Text));
          GameObject Inputmenu = new GameObject("InputField", typeof(Image), typeof(InputField));
          Inputmenu.GetComponent<RectTransform>().sizeDelta = new Vector2(160, 30);
          Inputmenu.GetComponent<RectTransform>().position = new Vector2(100, 0);
          Inputmenu.transform.SetParent(Button.transform);
          Inputmenu.GetComponent<Image>().sprite = InputFieldSpritee;
          Inputmenu.GetComponent<Image>().type = by.GetComponent<Image>().type;
          Inputmenu.GetComponent<InputField>().textComponent= TextInputmenu.GetComponent<Text>();

          TextInputmenu.transform.SetParent(Inputmenu.transform);
          TextInputmenu.GetComponent<RectTransform>().sizeDelta = new Vector2(160, 30);
          TextInputmenu.GetComponent<RectTransform>().position = new Vector2(101, 0);
          TextInputmenu.GetComponent<Text>().font = font;
          TextInputmenu.GetComponent<Text>().color = Color.black;
          TextInputmenu.GetComponent<Text>().alignment = TextAnchor.MiddleLeft;
      

      

        Kontragent.Add(new Items());
       
        Kontragent[index].alpfaform = Button.transform;
        index++;



    }

    public void CriateButtonWarehouse(Transform MyTransform)//Сотворить позицию склада
    {
        GameObject Button = new GameObject("Кнопка", typeof(Image), typeof(Button), typeof(LayoutElement));
        Button.transform.SetParent(MyTransform);
        Button.GetComponent<RectTransform>().sizeDelta = new Vector2(30, 30);
        // Button.GetComponent<RectTransform>().position = new Vector2(150, 280);
        Button.GetComponent<Image>().sprite = ButtonSpritee; //Resources.Load<Sprite>("Assets/UISprite");
        Button.GetComponent<Image>().type = by.GetComponent<Image>().type;
        Button.GetComponent<LayoutElement>().minHeight = 35;
        //   Button.GetComponent<Button>().onClick.AddListener(delegate { PressButton(); });

        GameObject TextButton = new GameObject("Text", typeof(Text));//текст кнопки
        TextButton.transform.SetParent(Button.transform);
        TextButton.GetComponent<RectTransform>().sizeDelta = new Vector2(30, 30);
        // TextButton.GetComponent<RectTransform>().position = new Vector2(160, 30);
        TextButton.GetComponent<Text>().text = index.ToString();
        TextButton.GetComponent<Text>().font = font;
        TextButton.GetComponent<Text>().color = Color.black;
        TextButton.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;

        GameObject TextInputmenu = new GameObject("Text", typeof(Text));
        GameObject Inputmenu = new GameObject("InputField", typeof(Image), typeof(InputField));
        Inputmenu.GetComponent<RectTransform>().sizeDelta = new Vector2(160, 30);
        Inputmenu.GetComponent<RectTransform>().position = new Vector2(100, 0);
        Inputmenu.transform.SetParent(Button.transform);
        Inputmenu.GetComponent<Image>().sprite = InputFieldSpritee;
        Inputmenu.GetComponent<Image>().type = by.GetComponent<Image>().type;
        Inputmenu.GetComponent<InputField>().textComponent = TextInputmenu.GetComponent<Text>();

        TextInputmenu.transform.SetParent(Inputmenu.transform);
        TextInputmenu.GetComponent<RectTransform>().sizeDelta = new Vector2(160, 30);
        TextInputmenu.GetComponent<RectTransform>().position = new Vector2(101, 0);
        TextInputmenu.GetComponent<Text>().font = font;
        TextInputmenu.GetComponent<Text>().color = Color.black;
        TextInputmenu.GetComponent<Text>().alignment = TextAnchor.MiddleLeft;

    }

    public void CriateButtonСontractors(Transform MyTransform)//Сотворить
    {
        GameObject Button = new GameObject("Кнопка", typeof(Image), typeof(Button), typeof(LayoutElement));
        Button.transform.SetParent(MyTransform);
        Button.GetComponent<RectTransform>().sizeDelta = new Vector2(30, 30);
        // Button.GetComponent<RectTransform>().position = new Vector2(150, 280);
        Button.GetComponent<Image>().sprite = ButtonSpritee; //Resources.Load<Sprite>("Assets/UISprite");
        Button.GetComponent<Image>().type = by.GetComponent<Image>().type;
        Button.GetComponent<LayoutElement>().minHeight = 35;
        //   Button.GetComponent<Button>().onClick.AddListener(delegate { PressButton(); });

        GameObject TextButton = new GameObject("Text", typeof(Text));//текст кнопки
        TextButton.transform.SetParent(Button.transform);
        TextButton.GetComponent<RectTransform>().sizeDelta = new Vector2(30, 30);
        // TextButton.GetComponent<RectTransform>().position = new Vector2(160, 30);
        TextButton.GetComponent<Text>().text = index.ToString();
        TextButton.GetComponent<Text>().font = font;
        TextButton.GetComponent<Text>().color = Color.black;
        TextButton.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;

        GameObject TextInputmenu = new GameObject("Text", typeof(Text));
        GameObject Inputmenu = new GameObject("InputField", typeof(Image), typeof(InputField));
        Inputmenu.GetComponent<RectTransform>().sizeDelta = new Vector2(160, 30);
        Inputmenu.GetComponent<RectTransform>().position = new Vector2(100, 0);
        Inputmenu.transform.SetParent(Button.transform);
        Inputmenu.GetComponent<Image>().sprite = InputFieldSpritee;
        Inputmenu.GetComponent<Image>().type = by.GetComponent<Image>().type;
        Inputmenu.GetComponent<InputField>().textComponent = TextInputmenu.GetComponent<Text>();

        TextInputmenu.transform.SetParent(Inputmenu.transform);
        TextInputmenu.GetComponent<RectTransform>().sizeDelta = new Vector2(160, 30);
        TextInputmenu.GetComponent<RectTransform>().position = new Vector2(101, 0);
        TextInputmenu.GetComponent<Text>().font = font;
        TextInputmenu.GetComponent<Text>().color = Color.black;
        TextInputmenu.GetComponent<Text>().alignment = TextAnchor.MiddleLeft;
        /////////////////////////////////////////////////////////////////////////////////
        GameObject TextInputmenuPrise = new GameObject("Text", typeof(Text));
        GameObject InputmenuPrise = new GameObject("InputField", typeof(Image), typeof(InputField));
        InputmenuPrise.GetComponent<RectTransform>().sizeDelta = new Vector2(40, 30);
        InputmenuPrise.GetComponent<RectTransform>().position = new Vector2(205, 0);
        InputmenuPrise.transform.SetParent(Button.transform);
        InputmenuPrise.GetComponent<Image>().sprite = InputFieldSpritee;
        InputmenuPrise.GetComponent<Image>().type = by.GetComponent<Image>().type;
        InputmenuPrise.GetComponent<InputField>().textComponent = TextInputmenuPrise.GetComponent<Text>();
        InputmenuPrise.GetComponent<InputField>().contentType = InputField.ContentType.IntegerNumber;

        TextInputmenuPrise.transform.SetParent(InputmenuPrise.transform);
        TextInputmenuPrise.GetComponent<RectTransform>().sizeDelta = new Vector2(40, 30);
        TextInputmenuPrise.GetComponent<RectTransform>().position = new Vector2(205, 0);
        TextInputmenuPrise.GetComponent<Text>().font = font;
        TextInputmenuPrise.GetComponent<Text>().color = Color.black;
        TextInputmenuPrise.GetComponent<Text>().alignment = TextAnchor.MiddleLeft;





    }

    public void CloseWindow()
    {
        transform.parent.gameObject.SetActive(false);
    }

    public void SetTheColorsObjects(Transform bufer, List<Items> array, int index)
    {
        if(array[index].isDebts && !array[index].isAlien)
        {
            bufer.Find("InputFieldName").GetComponent<Image>().color =  new Color(255/255.0f,223/255.0f,1/255.0f);  
            bufer.Find("InputFieldTele").GetComponent<Image>().color =  new Color(255/255.0f,223/255.0f,1/255.0f);  
            bufer.Find("InputFieldSum").GetComponent<Image>().color =  new Color(255/255.0f,223/255.0f,1/255.0f);  
            bufer.Find("Close").GetComponent<Image>().color =  new Color(255/255.0f,223/255.0f,1/255.0f);
            
        }

        if(!array[index].isDebts && array[index].isAlien)
        {
            bufer.Find("InputFieldName").GetComponent<Image>().color =  new Color(255/255.0f,100/255.0f,100/255.0f);  
            bufer.Find("InputFieldTele").GetComponent<Image>().color =  new Color(255/255.0f,100/255.0f,100/255.0f);  
            bufer.Find("InputFieldSum").GetComponent<Image>().color =  new Color(255/255.0f,100/255.0f,100/255.0f);  
            bufer.Find("Close").GetComponent<Image>().color =  new Color(255/255.0f,100/255.0f,100/255.0f);                   
        }
        
    }
    public IEnumerator DestroyObjectAndArray(GameObject Object,List<Items> array ,int index )
    {
        yield return new WaitForSeconds(0);
        array.RemoveAt(index);
        Destroy(Object.gameObject);
        for (int x = 0; x < array.Count; x++)
        {
            array[x].Polatrans.GetChild(0).GetComponent<Text>().text=x.ToString();//тут мы пересчитываем номерацию
        }
        //Debug.Log("удаляем");
        Podschet();
    }
   
    public void destroy(GameObject Obj)
    {
        //Debug.Log("destroy");
        if(Obj.name == "Кнопка(Clone)")
        {
            for (int i = 0; i < polaReis.Count; i++)
            {
                if(polaReis[i].Polatrans.gameObject == Obj.gameObject)
                {                
                    polaReis.RemoveAt(int.Parse(Obj.transform.GetChild(0).GetComponent<Text>().text));        
            
                    Destroy(Obj.gameObject);
                    reis1Index--;
                    for (int x = 0; x < polaReis.Count; x++)
                    {
                        polaReis[x].Polatrans.GetChild(0).GetComponent<Text>().text=x.ToString();//тут мы пересчитываем номерацию
                    }
                }           
            }       

            for (int i = 0; i < polaReis2.Count; i++)
            {
                if(polaReis2[i].Polatrans.gameObject==Obj.gameObject)
                {                
                    polaReis2.RemoveAt(int.Parse(Obj.transform.GetChild(0).GetComponent<Text>().text));        
            
                    Destroy(Obj.gameObject);
                    reis2Index--;
                    for (int x = 0; x < polaReis2.Count; x++)
                    {
                        polaReis2[x].Polatrans.GetChild(0).GetComponent<Text>().text=x.ToString();//тут мы пересчитываем номерацию
                    }
                }            
            } 
            // Debug.Log(debtor.Count+" вход");


            for (int i = 0; i < debtor.Count; i++)
            {           
            
                if(debtor[i].Adress == Obj.transform.GetChild(1).GetComponent<InputField>().text && debtor[i].date == data )
                {
                    Destroy(debtor[i].Polatrans.gameObject);
                    debtor.RemoveAt(i);               
                    debtorIndex--;
                    for (int x = 0; x < debtor.Count; x++)
                    {
                        debtor[x].Polatrans.GetChild(0).GetComponent<Text>().text=x.ToString();//тут мы пересчитываем номерацию
                    } 
                    i--;
                }
            }      
        }
        else
        if(Obj.name == "Кнопка Долгов(Clone)")
        {
            for (int i = 0; i < debtor.Count; i++)
            {
                if(Obj.gameObject == debtor[i].Polatrans.gameObject)
                {                    
                    Destroy(debtor[i].Polatrans.gameObject);
                    debtor.RemoveAt(i);               
                    debtorIndex--;
                    for (int x = 0; x < debtor.Count; x++)
                    {
                        debtor[x].Polatrans.GetChild(0).GetComponent<Text>().text=x.ToString();//тут мы пересчитываем номерацию
                    } 
                    
                }
            }    
        }
        else
        if(Obj.name == "Кнопка")
        {
            Kontragent.RemoveAt(int.Parse(Obj.transform.GetChild(0).GetComponent<Text>().text));        
            
            Destroy(Obj.gameObject);
            index--;
            IndeXCriatePrefab--;
            for (int x = 0; x < Kontragent.Count; x++)
            {
                Kontragent[x].alpfaform.GetChild(0).GetComponent<Text>().text=x.ToString();//тут мы пересчитываем номерацию
            }
        }
    }
    
    public void Save()
    {
        for (int i = 0; i < Kontragent.Count; i++)
        {
            if(transform.parent.Find("Меню Типа цен/Identidicater").GetComponent<Text>().text==Kontragent[i].Adress)
            {
                Kontragent[i].isDebts=transform.parent.Find("Меню Типа цен/Безнал").GetComponent<Toggle>().isOn;
                Kontragent[i].isAlien=transform.parent.Find("Меню Типа цен/Чужой").GetComponent<Toggle>().isOn;
            }
        }
       
        StreamWriter sw = new StreamWriter("SaveBase/kontragent.txt"); //файл для сохранения данных контрагентов
        
        /////////////////////////////////////////////////////////сохраняем контрагентов
        sw.WriteLine(index);
        for (int i = 0; i < index; i++)
        {
            sw.WriteLine(Kontragent[i].Adress);
            sw.WriteLine(Kontragent[i].telephone);

            sw.WriteLine(Kontragent[i].darina);
            //sw.WriteLine(Kontragent[i].darinaSum);
            sw.WriteLine(Kontragent[i].darinaZakup);

            sw.WriteLine(Kontragent[i].GornoyKrest);
            //sw.WriteLine(Kontragent[i].GornoyKrestSum); 
            sw.WriteLine(Kontragent[i].GornoyKrestZakup);   

            sw.WriteLine(Kontragent[i].Dombay);
            //sw.WriteLine(Kontragent[i].DombaySum);
            sw.WriteLine(Kontragent[i].DombayZakup);

            sw.WriteLine(Kontragent[i].GornoyVersh);
            //sw.WriteLine(Kontragent[i].GornoyVershSum);
            sw.WriteLine(Kontragent[i].GornoyVershZakup);

            sw.WriteLine(Kontragent[i].Pilegrim);
            //sw.WriteLine(Kontragent[i].PilegrimSum);
            sw.WriteLine(Kontragent[i].PilegrimZakup);

            sw.WriteLine(Kontragent[i].Kubay);
            //sw.WriteLine(Kontragent[i].KubaySum);
            sw.WriteLine(Kontragent[i].KubayZakup);

            sw.WriteLine(Kontragent[i].Tanais);
            //sw.WriteLine(Kontragent[i].TanaisSum);
            sw.WriteLine(Kontragent[i].TanaisZakup);

            sw.WriteLine(Kontragent[i].WorkPrise);
            sw.WriteLine(Kontragent[i].isDebts);
            sw.WriteLine(Kontragent[i].isAlien);
            sw.WriteLine(Kontragent[i].edidObjekt);
        }

        sw.WriteLine(debtorIndex);
        for (int i = 0; i < debtorIndex; i++)
        {
            sw.WriteLine(debtor[i].Adress);
            sw.WriteLine(debtor[i].date);
            sw.WriteLine(debtor[i].debt);
            sw.WriteLine(debtor[i].red);
        }

        //Debug.Log("save");
        sw.Close();
        /////////////////////////////////////////////////////////

        /////////////////////////////////////////////////////////сохраняем все подряд
        StreamWriter save = new StreamWriter("SaveBase/Save.txt");
        
        save.WriteLine(Vidindex -1);
        save.WriteLine(fcp.hexInput.text);
        save.WriteLine(fcp.color);

        save.WriteLine(IndeXCriateNomenklatura);
        for (int i = 0; i < IndeXCriateNomenklatura; i++)
        {           
            save.WriteLine(OptionNomen[i].transform.Find("InputFieldName").GetComponent<InputField>().text);
        }
        save.WriteLine(cassa);
        save.Close();
        /////////////////////////////////////////////////////////

        /////////////////////////////////////////////////////////
        StreamWriter SaveData = new StreamWriter("SaveBase/SaveData/"+data+".txt");//сохраняем маршрутник 

        SaveData.WriteLine(reis1Index);
        for (int i = 0; i < reis1Index; i++)
        {
            SaveData.WriteLine(polaReis[i].Adress);
            SaveData.WriteLine(polaReis[i].telephone);

            SaveData.WriteLine(polaReis[i].darina);
            SaveData.WriteLine(polaReis[i].darinaSum);
            SaveData.WriteLine(polaReis[i].darinaZakup);

            SaveData.WriteLine(polaReis[i].GornoyKrest);
            SaveData.WriteLine(polaReis[i].GornoyKrestSum);                       
            SaveData.WriteLine(polaReis[i].GornoyKrestZakup);  

            SaveData.WriteLine(polaReis[i].Dombay);
            SaveData.WriteLine(polaReis[i].DombaySum);
            SaveData.WriteLine(polaReis[i].DombayZakup);

            SaveData.WriteLine(polaReis[i].GornoyVersh);
            SaveData.WriteLine(polaReis[i].GornoyVershSum);
            SaveData.WriteLine(polaReis[i].GornoyVershZakup);

            SaveData.WriteLine(polaReis[i].Pilegrim);
            SaveData.WriteLine(polaReis[i].PilegrimSum);
            SaveData.WriteLine(polaReis[i].PilegrimZakup);

            SaveData.WriteLine(polaReis[i].Kubay);
            SaveData.WriteLine(polaReis[i].KubaySum);
            SaveData.WriteLine(polaReis[i].KubayZakup);

            SaveData.WriteLine(polaReis[i].Tanais);
            SaveData.WriteLine(polaReis[i].TanaisSum);
            SaveData.WriteLine(polaReis[i].TanaisZakup);

            SaveData.WriteLine(polaReis[i].WorkPrise);
            SaveData.WriteLine(polaReis[i].isDebts);
            SaveData.WriteLine(polaReis[i].isAlien);
            SaveData.WriteLine(polaReis[i].edidObjekt);
        }

         SaveData.WriteLine(reis2Index);
        for (int i = 0; i < reis2Index; i++)
        {
            SaveData.WriteLine(polaReis2[i].Adress);
            SaveData.WriteLine(polaReis2[i].telephone);

            SaveData.WriteLine(polaReis2[i].darina);
            SaveData.WriteLine(polaReis2[i].darinaSum);
            SaveData.WriteLine(polaReis2[i].darinaZakup);

            SaveData.WriteLine(polaReis2[i].GornoyKrest);
            SaveData.WriteLine(polaReis2[i].GornoyKrestSum);                       
            SaveData.WriteLine(polaReis2[i].GornoyKrestZakup);  

            SaveData.WriteLine(polaReis2[i].Dombay);
            SaveData.WriteLine(polaReis2[i].DombaySum);
            SaveData.WriteLine(polaReis2[i].DombayZakup);

            SaveData.WriteLine(polaReis2[i].GornoyVersh);
            SaveData.WriteLine(polaReis2[i].GornoyVershSum);
            SaveData.WriteLine(polaReis2[i].GornoyVershZakup);

            SaveData.WriteLine(polaReis2[i].Pilegrim);
            SaveData.WriteLine(polaReis2[i].PilegrimSum);
            SaveData.WriteLine(polaReis2[i].PilegrimZakup);

            SaveData.WriteLine(polaReis2[i].Kubay);
            SaveData.WriteLine(polaReis2[i].KubaySum);
            SaveData.WriteLine(polaReis2[i].KubayZakup);

            SaveData.WriteLine(polaReis2[i].Tanais);
            SaveData.WriteLine(polaReis2[i].TanaisSum);
            SaveData.WriteLine(polaReis2[i].TanaisZakup);

            SaveData.WriteLine(polaReis2[i].WorkPrise);
            SaveData.WriteLine(polaReis2[i].isDebts);
            SaveData.WriteLine(polaReis2[i].isAlien);
            SaveData.WriteLine(polaReis2[i].edidObjekt);
        }
        
        SaveData.WriteLine(transform.parent.Find("Меню Долги/долги тары/на начало").GetComponent<InputField>().text);
        SaveData.WriteLine(transform.parent.Find("Меню Долги/долги тары/купили на складе").GetComponent<InputField>().text);
        SaveData.WriteLine(transform.parent.Find("Меню Долги/долги тары/сережа привез").GetComponent<InputField>().text);
        SaveData.WriteLine(transform.parent.Find("Меню Долги/долги тары/забрали с ларька").GetComponent<InputField>().text);
        SaveData.WriteLine(transform.parent.Find("Меню Долги/долги тары/итог/Text").GetComponent<Text>().text);        
        SaveData.WriteLine(transform.parent.Find("Меню Долги/долги тары/история операций/поле заполнения истории").GetComponent<Text>().text);
        SaveData.WriteLine(sumTaraVithet);

        if(reis1.GetChild(18).GetComponent<Image>().color == new Color(255/255.0f,100/255.0f,100/255.0f))
        {
            SaveData.WriteLine("красный1");
        }
        else
        {
            SaveData.WriteLine("белый1");
        }

        if(reis2.GetChild(18).GetComponent<Image>().color == new Color(255/255.0f,100/255.0f,100/255.0f))
        {
            SaveData.WriteLine("красный2");
        }
        else
        {
            SaveData.WriteLine("белый2");
        }
        
        SaveData.Close();
        /////////////////////////////////////////////////////////
    }
    
    public void Load()
    {
        StreamReader load = new StreamReader("SaveBase/kontragent.txt");//тут мы загружаем данные  контрагентов

        /////////////////////////////////////////////////////////загружаем контрагентов
        Transform pathCriatePrefab=SerchMenu.GetChild(0).transform;
        Transform pathCriateNomenk=ContentNomeklatura.GetChild(0).transform;
        int.TryParse(load.ReadLine(), out index);
                   
        for (int i = 0; i < index; i++)
        {
            
            Kontragent.Add(new Items());
           
            Kontragent[i].Adress=load.ReadLine();
            Kontragent[i].telephone=load.ReadLine();      

            int.TryParse(load.ReadLine(), out Kontragent[i].darina);
            int.TryParse(load.ReadLine(), out Kontragent[i].darinaZakup);

            int.TryParse(load.ReadLine(), out Kontragent[i].GornoyKrest);
            int.TryParse(load.ReadLine(), out Kontragent[i].GornoyKrestZakup);

            int.TryParse(load.ReadLine(), out Kontragent[i].Dombay);
            int.TryParse(load.ReadLine(), out Kontragent[i].DombayZakup);

            int.TryParse(load.ReadLine(), out Kontragent[i].GornoyVersh);
            int.TryParse(load.ReadLine(), out Kontragent[i].GornoyVershZakup);

            int.TryParse(load.ReadLine(), out Kontragent[i].Pilegrim);
            int.TryParse(load.ReadLine(), out Kontragent[i].PilegrimZakup);

            int.TryParse(load.ReadLine(), out Kontragent[i].Kubay);
            int.TryParse(load.ReadLine(), out Kontragent[i].KubayZakup);

            int.TryParse(load.ReadLine(), out Kontragent[i].Tanais);
            int.TryParse(load.ReadLine(), out Kontragent[i].TanaisZakup);

            int.TryParse(load.ReadLine(), out Kontragent[i].WorkPrise);

            bool.TryParse(load.ReadLine(), out Kontragent[i].isDebts);          
            bool.TryParse(load.ReadLine(), out Kontragent[i].isAlien);          
            bool.TryParse(load.ReadLine(), out Kontragent[i].edidObjekt);          

            LoadParametrKontragent(pathCriatePrefab,i);
           
        }    

        int.TryParse(load.ReadLine(), out debtorIndex);   //тут мы загружаем долги из файла...надо понять нужно ли      
        for (int i = 0; i < debtorIndex; i++)
        {
            debtor.Add(new Debts());
            debtor[i].Adress=load.ReadLine();
            debtor[i].date=load.ReadLine();
            int.TryParse(load.ReadLine(), out debtor[i].debt); 
            bool.TryParse(load.ReadLine(), out debtor[i].red); 
        }
        LoadParametrDebts(); 
                   
        isLoad=true;
        load.Close();
        
        ///////////////////////////////////////////////////////// 

        StreamReader loadSaveFile = new StreamReader("SaveBase/Save.txt");//тут мы загружаем данные 

       
        int.TryParse(loadSaveFile.ReadLine(), out Vidindex);        
        color=loadSaveFile.ReadLine();
        ColorAnton=GetColor(loadSaveFile.ReadLine());        
        
       
        int.TryParse(loadSaveFile.ReadLine(), out IndeXCriateNomenklatura); //3й параметр не загружался вот я и загрузил его так
        //int.TryParse(loadSaveFile.ReadLine(), out IndeXCriateNomenklatura); 
        
        MASObjectColor[5].gameObject.SetActive(true);
        for (int i = 0; i < IndeXCriateNomenklatura; i++)
        {           
            LoadParametrNomenklatura(pathCriateNomenk,i);
            OptionNomen[i].transform.GetChild(1).GetComponent<InputField>().text=loadSaveFile.ReadLine();
        }       
        
        NextBakcground();     

        MASObjectColor[5].gameObject.SetActive(false);
        int.TryParse(loadSaveFile.ReadLine(), out cassa);
        SerchPanel.parent.Find("Касса/Касса").GetComponent<Text>().text=cassa.ToString();
        Invoke("QuitOptions",0.2f); 

        loadSaveFile.Close();
        
        ///////////////////////////////////////////////////////// 
        
        ///////////////////////////////////////////////////////// 
        if(File.Exists("SaveBase/SaveData/"+data+".txt"))
        {
            StreamReader loadData = new StreamReader("SaveBase/SaveData/"+data+".txt");//тут мы загружаем из файла массив 
           
            int.TryParse(loadData.ReadLine(), out reis1Index); 
            for (int i = 0; i < reis1Index; i++)
            {
                polaReis.Add(new Items());

                polaReis[i].Adress=loadData.ReadLine();
                polaReis[i].telephone=loadData.ReadLine();   

                int.TryParse(loadData.ReadLine(), out polaReis[i].darina);
                int.TryParse(loadData.ReadLine(), out polaReis[i].darinaSum);
                int.TryParse(loadData.ReadLine(), out polaReis[i].darinaZakup);

                int.TryParse(loadData.ReadLine(), out polaReis[i].GornoyKrest);
                int.TryParse(loadData.ReadLine(), out polaReis[i].GornoyKrestSum);
                int.TryParse(loadData.ReadLine(), out polaReis[i].GornoyKrestZakup);

                int.TryParse(loadData.ReadLine(), out polaReis[i].Dombay);
                int.TryParse(loadData.ReadLine(), out polaReis[i].DombaySum);
                int.TryParse(loadData.ReadLine(), out polaReis[i].DombayZakup);

                int.TryParse(loadData.ReadLine(), out polaReis[i].GornoyVersh);
                int.TryParse(loadData.ReadLine(), out polaReis[i].GornoyVershSum);
                int.TryParse(loadData.ReadLine(), out polaReis[i].GornoyVershZakup);

                int.TryParse(loadData.ReadLine(), out polaReis[i].Pilegrim);
                int.TryParse(loadData.ReadLine(), out polaReis[i].PilegrimSum);
                int.TryParse(loadData.ReadLine(), out polaReis[i].PilegrimZakup);

                int.TryParse(loadData.ReadLine(), out polaReis[i].Kubay);
                int.TryParse(loadData.ReadLine(), out polaReis[i].KubaySum);
                int.TryParse(loadData.ReadLine(), out polaReis[i].KubayZakup);

                int.TryParse(loadData.ReadLine(), out polaReis[i].Tanais);
                int.TryParse(loadData.ReadLine(), out polaReis[i].TanaisSum);
                int.TryParse(loadData.ReadLine(), out polaReis[i].TanaisZakup);

                int.TryParse(loadData.ReadLine(), out polaReis[i].WorkPrise);
                bool.TryParse(loadData.ReadLine(), out polaReis[i].isDebts);  
                bool.TryParse(loadData.ReadLine(), out polaReis[i].isAlien);  
                bool.TryParse(loadData.ReadLine(), out polaReis[i].edidObjekt);  

                //LoadParametrReis1(i);
                LoadParametrReis(polaReis, i, transform.parent.Find("Меню Рейс №1/Scrol/Content"));
             
                       
            }

            int.TryParse(loadData.ReadLine(), out reis2Index); 
            for (int i = 0; i < reis2Index; i++)
            {
                polaReis2.Add(new Items());

                polaReis2[i].Adress=loadData.ReadLine();
                polaReis2[i].telephone=loadData.ReadLine();   

                int.TryParse(loadData.ReadLine(), out polaReis2[i].darina);
                int.TryParse(loadData.ReadLine(), out polaReis2[i].darinaSum);
                int.TryParse(loadData.ReadLine(), out polaReis2[i].darinaZakup);

                int.TryParse(loadData.ReadLine(), out polaReis2[i].GornoyKrest);
                int.TryParse(loadData.ReadLine(), out polaReis2[i].GornoyKrestSum);
                int.TryParse(loadData.ReadLine(), out polaReis2[i].GornoyKrestZakup);

                int.TryParse(loadData.ReadLine(), out polaReis2[i].Dombay);
                int.TryParse(loadData.ReadLine(), out polaReis2[i].DombaySum);
                int.TryParse(loadData.ReadLine(), out polaReis2[i].DombayZakup);

                int.TryParse(loadData.ReadLine(), out polaReis2[i].GornoyVersh);
                int.TryParse(loadData.ReadLine(), out polaReis2[i].GornoyVershSum);
                int.TryParse(loadData.ReadLine(), out polaReis2[i].GornoyVershZakup);

                int.TryParse(loadData.ReadLine(), out polaReis2[i].Pilegrim);
                int.TryParse(loadData.ReadLine(), out polaReis2[i].PilegrimSum);
                int.TryParse(loadData.ReadLine(), out polaReis2[i].PilegrimZakup);

                int.TryParse(loadData.ReadLine(), out polaReis2[i].Kubay);
                int.TryParse(loadData.ReadLine(), out polaReis2[i].KubaySum);
                int.TryParse(loadData.ReadLine(), out polaReis2[i].KubayZakup);

                int.TryParse(loadData.ReadLine(), out polaReis2[i].Tanais);
                int.TryParse(loadData.ReadLine(), out polaReis2[i].TanaisSum);
                int.TryParse(loadData.ReadLine(), out polaReis2[i].TanaisZakup);

                int.TryParse(loadData.ReadLine(), out polaReis2[i].WorkPrise);
                bool.TryParse(loadData.ReadLine(), out polaReis2[i].isDebts);
                bool.TryParse(loadData.ReadLine(), out polaReis2[i].isAlien);
                bool.TryParse(loadData.ReadLine(), out polaReis2[i].edidObjekt);

                //LoadParametrReis2(i);
                LoadParametrReis(polaReis2, i, transform.parent.Find("Меню Рейс №2/Scrol/Content"));
                       
            }
            Podschet();          

            transform.parent.Find("Меню Долги/долги тары/на начало").GetComponent<InputField>().text = loadData.ReadLine();
            transform.parent.Find("Меню Долги/долги тары/купили на складе").GetComponent<InputField>().text = loadData.ReadLine();
            transform.parent.Find("Меню Долги/долги тары/сережа привез").GetComponent<InputField>().text = loadData.ReadLine();
            transform.parent.Find("Меню Долги/долги тары/забрали с ларька").GetComponent<InputField>().text = loadData.ReadLine();
            transform.parent.Find("Меню Долги/долги тары/итог/Text").GetComponent<Text>().text = loadData.ReadLine();
            transform.parent.Find("Меню Долги/долги тары/история операций/поле заполнения истории").GetComponent<Text>().text = loadData.ReadLine();
            int.TryParse(loadData.ReadLine(), out sumTaraVithet);            
            transform.parent.Find("Меню Долги/долги тары/общ сдали/Text").GetComponent<Text>().text = sumTaraVithet.ToString();

            String buferReis1 = loadData.ReadLine();
            if(buferReis1 == "красный1")
            {
                reis1.GetChild(18).GetComponent<Image>().color = new Color(255/255.0f,100/255.0f,100/255.0f);
            }
            else
            {
                reis1.GetChild(18).GetComponent<Image>().color = new Color(255/255.0f,255/255.0f,255/255.0f);
            }
            
            String buferReis2 = loadData.ReadLine();
            if(buferReis2 == "красный2")
            {
                reis2.GetChild(18).GetComponent<Image>().color = new Color(255/255.0f,100/255.0f,100/255.0f);
            }
            else
            {
                reis2.GetChild(18).GetComponent<Image>().color = new Color(255/255.0f,255/255.0f,255/255.0f);
            }

            loadData.Close();
        }
    }

    public void LoadParametrReis(List<Items> array, int i, Transform path)//подгружаем из массива в рейс из файла при запуске программы
    {
       
            Transform bufer;

            bufer = Instantiate(PrefabNomenReis);
            bufer.SetParent(path);
            
            
            bufer.Find("Text").GetComponent<Text>().text=i.ToString();
                        
            bufer.Find("InputFieldName").GetComponent<InputField>().text=
            array[i].Adress;            
           
            bufer.Find("InputFieldTele").GetComponent<InputField>().text=
            array[i].telephone.ToString();

            bufer.Find("InputFieldDarinaPrice/TextPrice").GetComponent<Text>().text="X "+array[i].darina.ToString(); 
            if(array[i].darinaSum != 0)
            {
                bufer.Find("InputFieldDarinaPrice").GetComponent<InputField>().text = array[i].darinaSum.ToString();    
            } else bufer.Find("InputFieldDarinaPrice").GetComponent<InputField>().text = "";
             
            bufer.Find("InputFieldGKPrice/TextPrice").GetComponent<Text>().text="X "+array[i].GornoyKrest.ToString(); 
            if(array[i].GornoyKrestSum != 0)
            {
                bufer.Find("InputFieldGKPrice").GetComponent<InputField>().text = array[i].GornoyKrestSum.ToString();    
            } else bufer.Find("InputFieldGKPrice").GetComponent<InputField>().text = "";
            
            bufer.Find("InputFieldDMPrice/TextPrice").GetComponent<Text>().text="X "+array[i].Dombay.ToString();
            if(array[i].DombaySum != 0)
            {
                bufer.Find("InputFieldDMPrice").GetComponent<InputField>().text = array[i].DombaySum.ToString();    
            } else bufer.Find("InputFieldDMPrice").GetComponent<InputField>().text = "";
           
            bufer.Find("InputFieldGVPrice/TextPrice").GetComponent<Text>().text="X "+array[i].GornoyVersh.ToString();
            if(array[i].GornoyVershSum != 0)
            {
                bufer.Find("InputFieldGVPrice").GetComponent<InputField>().text = array[i].GornoyVershSum.ToString();    
            } else bufer.Find("InputFieldGVPrice").GetComponent<InputField>().text = "";
            
            bufer.Find("InputFieldPIPrice/TextPrice").GetComponent<Text>().text="X "+array[i].Pilegrim.ToString();
            if(array[i].PilegrimSum != 0)
            {
                bufer.Find("InputFieldPIPrice").GetComponent<InputField>().text = array[i].PilegrimSum.ToString();    
            } else bufer.Find("InputFieldPIPrice").GetComponent<InputField>().text = "";
           
            bufer.Find("InputFieldKUPrice/TextPrice").GetComponent<Text>().text="X "+array[i].Kubay.ToString();
            if(array[i].KubaySum != 0)
            {
                bufer.Find("InputFieldKUPrice").GetComponent<InputField>().text = array[i].KubaySum.ToString();    
            } else bufer.Find("InputFieldKUPrice").GetComponent<InputField>().text = "";

            bufer.Find("InputFieldTNPrice/TextPrice").GetComponent<Text>().text="X "+array[i].Tanais.ToString();
            if(array[i].TanaisSum != 0)
            {
                bufer.Find("InputFieldTNPrice").GetComponent<InputField>().text = array[i].TanaisSum.ToString();    
            } else bufer.Find("InputFieldTNPrice").GetComponent<InputField>().text = "";
            
            
            array[i].Polatrans=bufer; 
            if(array[i].edidObjekt == false)
            {
                array[i].Polatrans.Find("Close").gameObject.SetActive(false);
            }
            if(array[i].isDebts)
            {                  
                array[i].Polatrans.GetChild(1).GetComponent<Image>().color  =  new Color(255/255.0f,223/255.0f,1/255.0f);  
                array[i].Polatrans.GetChild(2).GetComponent<Image>().color  =  new Color(255/255.0f,223/255.0f,1/255.0f);  
                array[i].Polatrans.GetChild(10).GetComponent<Image>().color =  new Color(255/255.0f,223/255.0f,1/255.0f);  
                array[i].Polatrans.GetChild(11).GetComponent<Image>().color =  new Color(255/255.0f,223/255.0f,1/255.0f);  
                AddDebts(array[i]);
            }
            
            if(array[i].isAlien)
            {                  
                array[i].Polatrans.GetChild(1).GetComponent<Image>().color  =  new Color(255/255.0f,100/255.0f,100/255.0f);   
                array[i].Polatrans.GetChild(2).GetComponent<Image>().color  =  new Color(255/255.0f,100/255.0f,100/255.0f);   
                array[i].Polatrans.GetChild(10).GetComponent<Image>().color =  new Color(255/255.0f,100/255.0f,100/255.0f);   
                array[i].Polatrans.GetChild(11).GetComponent<Image>().color =  new Color(255/255.0f,100/255.0f,100/255.0f);   
                AddDebts(array[i]);
            }      
    }
    

    public void LoadParametrKontragent(Transform MyTransform,int i)//такая же функция как и  CriatePrefab только без увеличения индекса 
    { 
        Transform Pref;
        Pref=Instantiate(PrefabKontragent);
        Pref.GetChild(0).GetComponent<Text>().text= IndeXCriatePrefab.ToString();//копируем порядочный номер в игровой объект
        IndeXCriatePrefab++;//порядочный номер

        Pref.name = "Кнопка";
        Pref.SetParent(MyTransform);     

        //Vidplayer.clip = VidClip[Vidindex];
       // Debug.Log(Vidindex);
        Kontragent[i].alpfaform=Pref;
        Pref.GetChild(1).GetComponent<InputField>().text=Kontragent[i].Adress;
        Pref.GetChild(2).GetComponent<InputField>().text=Kontragent[i].telephone;
        
    }

    public void LoadParametrNomenklatura(Transform MyTransform,int i)//такая же функция как и  CriatePrefab только без увеличения индекса 
    {
        ///////////////////////////////Создаем поля в меню номенклатуры
        Transform Pref;
        Pref=Instantiate(PrefabNomenklatura);

        Pref.GetChild(0).GetComponent<Text>().text = i.ToString();
        Pref.name = "Номенклатура";
        Pref.SetParent(MyTransform);        
      
        ////////////////////////////////Создаем поля в меню типы цен
        Pref = Instantiate(PrefabNomenklatura);

        Pref.GetChild(0).GetComponent<Text>().text = i.ToString();
        Pref.SetParent(transform.parent.Find("Меню Типа цен/Scrol/Content"));
        Pref.tag = "Nomen";
        ///////////////////////////////
        FindKontragent();
    }

    public void LoadCalendarDay(Text DataDay )//загружает день когда тыкаешь на календарь
    {
        ResrtTara();
        reis1.GetChild(5).GetComponent<InputField>().text=0.ToString();// тут мы обнуляем все числовые параметры дня
        reis1.GetChild(8).GetComponent<InputField>().text=0.ToString();
        reis1.GetChild(9).GetComponent<InputField>().text=0.ToString();
        reis1.GetChild(10).GetComponent<InputField>().text=0.ToString();
        reis1.GetChild(11).GetComponent<InputField>().text=0.ToString();
        reis1.GetChild(12).GetComponent<InputField>().text=0.ToString();
        reis1.GetChild(13).GetComponent<InputField>().text=0.ToString();
        reis1.GetChild(14).GetComponent<InputField>().text=0.ToString();
        reis1.GetChild(15).GetComponent<InputField>().text=0.ToString();//

        reis2.GetChild(5).GetComponent<InputField>().text=0.ToString();// тут мы обнуляем все числовые параметры дня
        reis2.GetChild(8).GetComponent<InputField>().text=0.ToString();
        reis2.GetChild(9).GetComponent<InputField>().text=0.ToString();
        reis2.GetChild(10).GetComponent<InputField>().text=0.ToString();
        reis2.GetChild(11).GetComponent<InputField>().text=0.ToString();
        reis2.GetChild(12).GetComponent<InputField>().text=0.ToString();
        reis2.GetChild(13).GetComponent<InputField>().text=0.ToString();
        reis2.GetChild(14).GetComponent<InputField>().text=0.ToString();
        reis2.GetChild(15).GetComponent<InputField>().text=0.ToString();//

        for (int i = 0; i < reis1Index; i++)
        {
            Destroy(polaReis[i].Polatrans.gameObject);                   
        }

        for (int i = 0; i < reis2Index; i++)
        {
            Destroy(polaReis2[i].Polatrans.gameObject);                   
        }
        polaReis.Clear();
        polaReis2.Clear();
        reis1Index=0;
        reis2Index=0;

        String path=DataDay.GetComponent<Text>().text;
        data=path+"."+calendar.MouthAndYear.text;

        Debug.Log("файл этого дня "+File.Exists("SaveBase/SaveData/"+data+".txt"));

        if(File.Exists("SaveBase/SaveData/"+data+".txt")==true)
        {            
            StreamReader loadDay = new StreamReader("SaveBase/SaveData/"+data+".txt"); 

            int.TryParse(loadDay.ReadLine(), out reis1Index); 
            for (int i = 0; i < reis1Index; i++)
            {
                polaReis.Add(new Items());

                polaReis[i].Adress=loadDay.ReadLine();
                polaReis[i].telephone=loadDay.ReadLine();   

                int.TryParse(loadDay.ReadLine(), out polaReis[i].darina);
                int.TryParse(loadDay.ReadLine(), out polaReis[i].darinaSum);
                int.TryParse(loadDay.ReadLine(), out polaReis[i].darinaZakup);

                int.TryParse(loadDay.ReadLine(), out polaReis[i].GornoyKrest);
                int.TryParse(loadDay.ReadLine(), out polaReis[i].GornoyKrestSum);
                int.TryParse(loadDay.ReadLine(), out polaReis[i].GornoyKrestZakup);

                int.TryParse(loadDay.ReadLine(), out polaReis[i].Dombay);
                int.TryParse(loadDay.ReadLine(), out polaReis[i].DombaySum);
                int.TryParse(loadDay.ReadLine(), out polaReis[i].DombayZakup);

                int.TryParse(loadDay.ReadLine(), out polaReis[i].GornoyVersh);
                int.TryParse(loadDay.ReadLine(), out polaReis[i].GornoyVershSum);
                int.TryParse(loadDay.ReadLine(), out polaReis[i].GornoyVershZakup);

                int.TryParse(loadDay.ReadLine(), out polaReis[i].Pilegrim);
                int.TryParse(loadDay.ReadLine(), out polaReis[i].PilegrimSum);
                int.TryParse(loadDay.ReadLine(), out polaReis[i].PilegrimZakup);

                int.TryParse(loadDay.ReadLine(), out polaReis[i].Kubay);
                int.TryParse(loadDay.ReadLine(), out polaReis[i].KubaySum);
                int.TryParse(loadDay.ReadLine(), out polaReis[i].KubayZakup);

                int.TryParse(loadDay.ReadLine(), out polaReis[i].Tanais);
                int.TryParse(loadDay.ReadLine(), out polaReis[i].TanaisSum);
                int.TryParse(loadDay.ReadLine(), out polaReis[i].TanaisZakup);

                int.TryParse(loadDay.ReadLine(), out polaReis[i].WorkPrise);
                bool.TryParse(loadDay.ReadLine(), out polaReis[i].isDebts);
                bool.TryParse(loadDay.ReadLine(), out polaReis[i].isAlien);
                bool.TryParse(loadDay.ReadLine(), out polaReis[i].edidObjekt);

                //LoadParametrReis1(i);      
                LoadParametrReis(polaReis, i, transform.parent.Find("Меню Рейс №1/Scrol/Content"));
                
            }
            
            int.TryParse(loadDay.ReadLine(), out reis2Index); 
            for (int i = 0; i < reis2Index; i++)
            {
                polaReis2.Add(new Items());

                polaReis2[i].Adress=loadDay.ReadLine();
                polaReis2[i].telephone=loadDay.ReadLine();   

                int.TryParse(loadDay.ReadLine(), out polaReis2[i].darina);
                int.TryParse(loadDay.ReadLine(), out polaReis2[i].darinaSum);
                int.TryParse(loadDay.ReadLine(), out polaReis2[i].darinaZakup);

                int.TryParse(loadDay.ReadLine(), out polaReis2[i].GornoyKrest);
                int.TryParse(loadDay.ReadLine(), out polaReis2[i].GornoyKrestSum);
                int.TryParse(loadDay.ReadLine(), out polaReis2[i].GornoyKrestZakup);

                int.TryParse(loadDay.ReadLine(), out polaReis2[i].Dombay);
                int.TryParse(loadDay.ReadLine(), out polaReis2[i].DombaySum);
                int.TryParse(loadDay.ReadLine(), out polaReis2[i].DombayZakup);

                int.TryParse(loadDay.ReadLine(), out polaReis2[i].GornoyVersh);
                int.TryParse(loadDay.ReadLine(), out polaReis2[i].GornoyVershSum);
                int.TryParse(loadDay.ReadLine(), out polaReis2[i].GornoyVershZakup);

                int.TryParse(loadDay.ReadLine(), out polaReis2[i].Pilegrim);
                int.TryParse(loadDay.ReadLine(), out polaReis2[i].PilegrimSum);
                int.TryParse(loadDay.ReadLine(), out polaReis2[i].PilegrimZakup);

                int.TryParse(loadDay.ReadLine(), out polaReis2[i].Kubay);
                int.TryParse(loadDay.ReadLine(), out polaReis2[i].KubaySum);
                int.TryParse(loadDay.ReadLine(), out polaReis2[i].KubayZakup);

                int.TryParse(loadDay.ReadLine(), out polaReis2[i].Tanais);
                int.TryParse(loadDay.ReadLine(), out polaReis2[i].TanaisSum);
                int.TryParse(loadDay.ReadLine(), out polaReis2[i].TanaisZakup);

                int.TryParse(loadDay.ReadLine(), out polaReis2[i].WorkPrise);
                bool.TryParse(loadDay.ReadLine(), out polaReis2[i].isDebts);
                bool.TryParse(loadDay.ReadLine(), out polaReis2[i].isAlien);
                bool.TryParse(loadDay.ReadLine(), out polaReis2[i].edidObjekt);

                //LoadParametrReis2(i);      
                LoadParametrReis(polaReis2, i, transform.parent.Find("Меню Рейс №2/Scrol/Content"));
                
            }
            
            Debug.Log("загружаю");
            
            
            transform.parent.Find("Меню Долги/долги тары/на начало").GetComponent<InputField>().text = loadDay.ReadLine();
            transform.parent.Find("Меню Долги/долги тары/купили на складе").GetComponent<InputField>().text = loadDay.ReadLine();
            transform.parent.Find("Меню Долги/долги тары/сережа привез").GetComponent<InputField>().text = loadDay.ReadLine();
            transform.parent.Find("Меню Долги/долги тары/забрали с ларька").GetComponent<InputField>().text = loadDay.ReadLine();
            transform.parent.Find("Меню Долги/долги тары/итог/Text").GetComponent<Text>().text = loadDay.ReadLine();
            transform.parent.Find("Меню Долги/долги тары/история операций/поле заполнения истории").GetComponent<Text>().text = loadDay.ReadLine();
            int.TryParse(loadDay.ReadLine(), out sumTaraVithet);
            transform.parent.Find("Меню Долги/долги тары/общ сдали/Text").GetComponent<Text>().text = sumTaraVithet.ToString();
            
            String buferReis1 = loadDay.ReadLine();
            if(buferReis1 == "красный1")
            {
                reis1.GetChild(18).GetComponent<Image>().color = new Color(255/255.0f,100/255.0f,100/255.0f);
            }
            else
            {
                reis1.GetChild(18).GetComponent<Image>().color = new Color(255/255.0f,255/255.0f,255/255.0f);
            }
            
            String buferReis2 = loadDay.ReadLine();
            if(buferReis2 == "красный2")
            {
                reis2.GetChild(18).GetComponent<Image>().color = new Color(255/255.0f,100/255.0f,100/255.0f);
            }
            else
            {
                reis2.GetChild(18).GetComponent<Image>().color = new Color(255/255.0f,255/255.0f,255/255.0f);
            }

            loadDay.Close();
            
        }
        Podschet(); 
    }
    
    public void chengColor()//метод изменяет цвет элементов после загрузки из файла 
    {
        fcp.hexInput.text = color;
        //fcp.transform.parent.gameObject.SetActive(false);
        //Debug.Log("изменен цвет");
    }

    public float va;
    public Color GetColor(string color)
      {
        int a = color.IndexOf('(') + 1;
        int b = color.Length - 1 - a;

        string[] values = color.Substring(a, b).Split(',');

       // var culture = System.Globalization.CultureInfo.GetCultureInfo("en-US");
         
        return new Color(
            float.Parse(values[1])/1000, float.Parse(values[3])/1000, float.Parse(values[5])/1000, float.Parse(values[7])/1000);
         
    }   
  
    public void AddDebts(Items debs )//метод добовляет долг контрагента в массив долгов и передает его в UI  
    {      
        
        Transform bufer;      
        bool newAdd = true;
        if(debs.isDebts && debs.edidObjekt == true || debs.isAlien && debs.edidObjekt == true)
        {         
            for (int i = 0; i < debtor.Count; i++)
            {
                if(debs.Adress == debtor[i].Adress && data == debtor[i].date)
                {
                    if(debtor[i].Polatrans.GetChild(4).GetComponent<Image>().color == new Color(0/255.0f,255/255.0f,26/255.0f))//зеленый
                    {
                        if(debs.isAlien)
                        {
                            debtor[i].debt = (debs.darinaSum + debs.GornoyKrestSum + debs.DombaySum + debs.GornoyVershSum + 
                                                debs.PilegrimSum + debs.KubaySum + debs.TanaisSum) * debs.WorkPrise;
                        }
                        else
                        {
                            debtor[i].debt=int.Parse(debs.Polatrans.GetChild(10).GetComponent<InputField>().text);
                        }
                        
                        debtor[i].Polatrans.GetChild(3).GetComponent<InputField>().text = debtor[i].debt.ToString();                        
                    }
                    else                     
                    {
                        debtor[i].debt=int.Parse(debs.Polatrans.GetChild(10).GetComponent<InputField>().text);
                        debtor[i].Polatrans.GetChild(3).GetComponent<InputField>().text = debtor[i].debt.ToString();                     
                    }
                    newAdd=false;
                }                
            }                
            if(newAdd)
            {              
                //Debug.Log("создал долг");
                bufer = Instantiate(PrefabDebtor);
                bufer.SetParent(transform.parent.Find("Меню Долги/Scrol/Content")); 
            
                debtor.Add(new Debts());
                
                debtor[debtorIndex].Polatrans = bufer;
                bufer.Find("Text").GetComponent<Text>().text = debtorIndex.ToString();

                debtor[debtorIndex].Adress = debs.Adress;
                bufer.Find("InputFieldNameDebet").GetComponent<InputField>().text = debtor[debtorIndex].Adress;

                debtor[debtorIndex].date = data;
                bufer.Find("date").GetComponent<InputField>().text = debtor[debtorIndex].date;
                
                debtor[debtorIndex].debt = int.Parse(debs.Polatrans.GetChild(10).GetComponent<InputField>().text);
                bufer.Find("debet").GetComponent<InputField>().text = debtor[debtorIndex].debt.ToString();

                if(debs.isDebts)
                {
                    bufer.Find("Button/Text").GetComponent<Text>().text = "забрать";
                    bufer.Find("Button").GetComponent<Image>().color =  new Color(0/255.0f,255/255.0f,26/255.0f);
                    debtor[debtorIndex].red=true;
                }
                else
                if(debs.isAlien)
                {
                    bufer.Find("Button/Text").GetComponent<Text>().text = "отдать";
                    bufer.Find("Button").GetComponent<Image>().color =  new Color(255/255.0f,100/255.0f,100/255.0f);
                     debtor[debtorIndex].red = true;

                    bufer = Instantiate(PrefabDebtor);
                    bufer.SetParent(transform.parent.Find("Меню Долги/Scrol/Content")); 
                
                    debtor.Add(new Debts());

                    debtorIndex++;

                    debtor[debtorIndex].Polatrans = bufer;
                    bufer.Find("Text").GetComponent<Text>().text=debtorIndex.ToString();

                    debtor[debtorIndex].Adress = debs.Adress;
                    bufer.Find("InputFieldNameDebet").GetComponent<InputField>().text = debtor[debtorIndex].Adress;

                    debtor[debtorIndex].date = data;
                    bufer.Find("date").GetComponent<InputField>().text=debtor[debtorIndex].date;
                    
                    debtor[debtorIndex].debt = (debs.darinaSum + debs.GornoyKrestSum + debs.DombaySum + debs.GornoyVershSum + 
                                                debs.PilegrimSum + debs.KubaySum + debs.TanaisSum) * debs.WorkPrise;                                                
                    bufer.Find("debet").GetComponent<InputField>().text = debtor[debtorIndex].debt.ToString();
                   
                    bufer.Find("Button/Text").GetComponent<Text>().text = "забрать";
                    bufer.Find("Button").GetComponent<Image>().color =  new Color(0/255.0f,255/255.0f,26/255.0f);
                    debtor[debtorIndex].red = false;
                   
                }
                debtorIndex++;
            }         
        }

        
    }    

    public void LoadParametrDebts()//метод добавляет в меню долги UI элементы после загрузки из файла
    {
        Transform bufer;  
        for (int i = 0; i < debtorIndex; i++)
        {
            bufer = Instantiate(PrefabDebtor);
            debtor[i].Polatrans = bufer;

            bufer.SetParent(transform.parent.Find("Меню Долги/Scrol/Content")); 
            bufer.Find("Text").GetComponent<Text>().text = debtorIndex.ToString();

            bufer.Find("InputFieldNameDebet").GetComponent<InputField>().text = debtor[i].Adress;            
            bufer.Find("date").GetComponent<InputField>().text = debtor[i].date; 

            bufer.Find("debet").GetComponent<InputField>().text = debtor[i].debt.ToString(); 
            if(!debtor[i].red)
            {
               
                bufer.Find("Button/Text").GetComponent<Text>().text="отдать";
                bufer.Find("Button").GetComponent<Image>().color =  new Color(255/255.0f,100/255.0f,100/255.0f);
            }
            else
            {
                bufer.Find("Button/Text").GetComponent<Text>().text="забрать";
                bufer.Find("Button").GetComponent<Image>().color =  new Color(0/255.0f,255/255.0f,26/255.0f);
            }
        }
    }

    public void InCassa (Transform MyTransform)
    {
        if(MyTransform.GetComponent<Image>().color == new Color(0/255.0f,255/255.0f,26/255.0f))
        {
           cassa += int.Parse(MyTransform.parent.Find("debet").GetComponent<InputField>().text);
        }
        else
        {
            cassa -= int.Parse(MyTransform.parent.Find("debet").GetComponent<InputField>().text);
        }

        StreamWriter SaveData = new StreamWriter("SaveBase/SaveData/"+data+".txt");//сохраняем маршрутник

        SaveData.Close();

        for (int i = 0; i < polaReis.Count; i++)
        {
            if(MyTransform.parent.Find("InputFieldNameDebet").GetComponent<InputField>().text == polaReis[i].Adress)
            {
                polaReis[i].edidObjekt = false;
                polaReis[i].Polatrans.Find("Close").gameObject.SetActive(false);
            }
        }

        for (int i = 0; i < polaReis2.Count; i++)
        {
            if(MyTransform.parent.Find("InputFieldNameDebet").GetComponent<InputField>().text == polaReis2[i].Adress)
            {
                polaReis2[i].edidObjekt = false;
                polaReis2[i].Polatrans.Find("Close").gameObject.SetActive(false);
            }
        }
        SerchPanel.parent.Find("Касса/Касса").GetComponent<Text>().text=cassa.ToString();
        
    }
    
    public void Cassa(Transform MyTransform)
    {
        if(MyTransform.GetComponent<InputField>().text == "" || MyTransform.GetComponent<InputField>().text == "-")
        {
            MyTransform.GetComponent<InputField>().text = "0";
        }
        
        cassa += int.Parse(MyTransform.GetComponent<InputField>().text);
        SerchPanel.parent.Find("Касса/Касса").GetComponent<Text>().text=cassa.ToString();
        MyTransform.GetComponent<InputField>().text = "0";
    }

    public void CassaPlusReis(Transform MyTransform)
    {
        if(MyTransform.parent.GetChild(18).GetComponent<Image>().color == new Color(255/255.0f,255/255.0f,255/255.0f))
        {
            cassa += int.Parse(MyTransform.GetComponent<InputField>().text);
            SerchPanel.parent.Find("Касса/Касса").GetComponent<Text>().text=cassa.ToString();
            MyTransform.parent.GetChild(18).GetComponent<Image>().color = new Color(255/255.0f,100/255.0f,100/255.0f);
            MyTransform.parent.GetChild(18).GetChild(0).GetComponent<Text>().text = "вернуть деньги";
        }
        else
        if(MyTransform.parent.GetChild(18).GetComponent<Image>().color == new Color(255/255.0f,100/255.0f,100/255.0f))
        {
            cassa -= int.Parse(MyTransform.GetComponent<InputField>().text);
            SerchPanel.parent.Find("Касса/Касса").GetComponent<Text>().text=cassa.ToString();
            MyTransform.parent.GetChild(18).GetComponent<Image>().color = new Color(255/255.0f,255/255.0f,255/255.0f);
            MyTransform.parent.GetChild(18).GetChild(0).GetComponent<Text>().text = "принять деньги";
        }
    }

    public void SumTara(Transform MyTransform)// метод суммирует и вычитает доли тары
    {
        
        int bufer = int.Parse(MyTransform.GetComponent<InputField>().text);
       
        if(MyTransform.GetComponent<InputField>().text == "" || MyTransform.GetComponent<InputField>().text == "-" )
        {
            MyTransform.GetComponent<InputField>().text = "0";
        }

        sumTara = 0; 
        if(MyTransform.name == "сдали с машины" && bufer != 0)
        {
            sumTaraVithet += bufer;
            
            MyTransform.parent.Find("итог/Text").GetComponent<Text>().text = sumTara.ToString();           
            MyTransform.GetComponent<InputField>().text = "0";
            
            MyTransform.parent.Find("общ сдали/Text").GetComponent<Text>().text = sumTaraVithet.ToString();
            MyTransform.parent.Find("история операций/поле заполнения истории").GetComponent<Text>().text = 
            MyTransform.parent.Find("история операций/поле заполнения истории").GetComponent<Text>().text + 
            " "+ "выгрузили " + bufer;
        }

        sumTara += int.Parse(MyTransform.parent.Find("на начало").GetComponent<InputField>().text) +
                    int.Parse(MyTransform.parent.Find("купили на складе").GetComponent<InputField>().text) +
                    int.Parse(MyTransform.parent.Find("сережа привез").GetComponent<InputField>().text) +
                    int.Parse(MyTransform.parent.Find("забрали с ларька").GetComponent<InputField>().text) - sumTaraVithet ;
            
        MyTransform.parent.Find("итог/Text").GetComponent<Text>().text = sumTara.ToString();
    }

    public void ResrtTara ()//метод сбрасывает настройки долга тары в 0
    {
        Transform path = transform.parent.Find("Меню Долги/долги тары");

        path.GetChild(2).GetComponent<InputField>().text = "0";
        path.GetChild(3).GetComponent<InputField>().text = "0";
        path.GetChild(4).GetComponent<InputField>().text = "0";
        path.GetChild(9).GetChild(0).GetComponent<Text>().text = "0";       
    }

    public void ClearHistory(Transform MyTransform)// метод очищает историю 
    {
        sumTaraVithet = 0;
        MyTransform.parent.Find("история операций/поле заполнения истории").GetComponent<Text>().text = "";
        MyTransform.parent.Find("общ сдали/Text").GetComponent<Text>().text = "0";
    }

    public void FinalPodsthet(Transform MyTransform) //подсчет результатов закупки 
    {
        int DR=0;
        int DRObj=0;
        int GK=0;
        int DM=0;
        int GV=0;
        int PL=0;
        int KU=0;
        int TN=0;
        int RES=0; // закупка
        int RESPRI=0;//общая прибыль
        int OBJKOlBUt=0;//общая кол - во бутылок
        for (int i = 0; i < polaReis.Count; i++)
        {
            if(!polaReis[i].isAlien && !polaReis[i].isDebts)
            {
                GK += polaReis[i].GornoyKrestSum;
                DM += polaReis[i].DombaySum;
                GV += polaReis[i].GornoyVershSum;
                PL += polaReis[i].PilegrimSum;
                KU += polaReis[i].KubaySum;
                TN += polaReis[i].TanaisSum;

                RES += polaReis[i].GornoyKrestSum * polaReis[i].GornoyKrestZakup +
                    polaReis[i].DombaySum * polaReis[i].DombayZakup +
                    polaReis[i].GornoyVershSum * polaReis[i].GornoyVershZakup +
                    polaReis[i].PilegrimSum * polaReis[i].PilegrimZakup +
                    polaReis[i].KubaySum * polaReis[i].KubayZakup +
                    polaReis[i].TanaisSum * polaReis[i].TanaisZakup; 
            }
            OBJKOlBUt += polaReis[i].darinaSum + polaReis[i].GornoyKrestSum + polaReis[i].DombaySum + polaReis[i].GornoyVershSum +
                             polaReis[i].PilegrimSum + polaReis[i].KubaySum + polaReis[i].TanaisSum; 
        }

        for (int i = 0; i < polaReis2.Count; i++)
        {            
            if(!polaReis2[i].isAlien && !polaReis2[i].isDebts)
            {
                GK += polaReis2[i].GornoyKrestSum;
                DM += polaReis2[i].DombaySum;
                GV += polaReis2[i].GornoyVershSum;
                PL += polaReis2[i].PilegrimSum;
                KU += polaReis2[i].KubaySum;
                TN += polaReis2[i].TanaisSum;

                RES += polaReis2[i].GornoyKrestSum * polaReis2[i].GornoyKrestZakup +
                    polaReis2[i].DombaySum * polaReis2[i].DombayZakup +
                    polaReis2[i].GornoyVershSum * polaReis2[i].GornoyVershZakup +
                    polaReis2[i].PilegrimSum * polaReis2[i].PilegrimZakup +
                    polaReis2[i].KubaySum * polaReis2[i].KubayZakup +
                    polaReis2[i].TanaisSum * polaReis2[i].TanaisZakup; 
            }
            OBJKOlBUt += polaReis2[i].darinaSum + polaReis2[i].GornoyKrestSum + polaReis2[i].DombaySum + polaReis2[i].GornoyVershSum +
                             polaReis2[i].PilegrimSum + polaReis2[i].KubaySum + polaReis2[i].TanaisSum; 
            
        }

        RESPRI = int.Parse(reis1.Find("Prib").GetComponent<InputField>().text) + int.Parse(reis2.Find("Prib").GetComponent<InputField>().text);
        DR = int.Parse(MyTransform.Find("ДР").GetComponent<InputField>().text); 
        
        RES += DR * 25;
       
        MyTransform.Find("ГК/Text").GetComponent<Text>().text = GK.ToString();
        MyTransform.Find("ДМ/Text").GetComponent<Text>().text = DM.ToString();
        MyTransform.Find("ГВ/Text").GetComponent<Text>().text = GV.ToString();
        MyTransform.Find("ПЛ/Text").GetComponent<Text>().text = PL.ToString();
        MyTransform.Find("КУ/Text").GetComponent<Text>().text = KU.ToString();
        MyTransform.Find("ТН/Text").GetComponent<Text>().text = TN.ToString();
        MyTransform.Find("сумма денег/Text").GetComponent<Text>().text = RES.ToString();

        MyTransform.Find("Меню прибыли/общ кол-во/Text").GetComponent<Text>().text = OBJKOlBUt.ToString();
        MyTransform.Find("Меню прибыли/общ прибыль/Text").GetComponent<Text>().text = RESPRI.ToString();
    }

    public void addTeleInfo(Transform MyTransform) //передает из рейса в массив рейс
    {
        
        if(MyTransform.parent.GetChild(1).GetComponent<InputField>().text == polaReis[int.Parse(MyTransform.parent.GetChild(0).GetComponent<Text>().text)].Adress)
        {
            polaReis[int.Parse(MyTransform.parent.GetChild(0).GetComponent<Text>().text)].telephone = MyTransform.GetComponent<InputField>().text;
        }

        if(MyTransform.parent.GetChild(1).GetComponent<InputField>().text == polaReis2[int.Parse(MyTransform.parent.GetChild(0).GetComponent<Text>().text)].Adress)
        {
            polaReis2[int.Parse(MyTransform.parent.GetChild(0).GetComponent<Text>().text)].telephone = MyTransform.GetComponent<InputField>().text;
        }
    }

    public IEnumerator ScrollRect()//перемещает список контрагентов вниз через 0,05 от секунды
    {           
        yield return new WaitForSeconds(0.05f);
        scrollRect.GetComponent<Scrollbar>().value = 0;
    }
    [System.Serializable]
   public class black
    {
       public int a;
    }
    public List<black> KontragentGET = new List<black>();
    public List<black> KontragentSET = new List<black>();
    public void adad()
    {
        KontragentGET[0] = KontragentSET[0];
        //KontragentSET.Add(KontragentGET[0]);
      //KontragentSET[KontragentSET.Count -1] = ListCopy(KontragentGET, 0, KontragentSET);
    }

    public Items ListCopy(List<Items> arrayGet, int indexGet ,List<Items> arraySet, int indexSet, Transform bufer = null)
    {
        arraySet[indexSet].Adress = arrayGet[indexGet].Adress;
        arraySet[indexSet].telephone = arrayGet[indexGet].telephone;
        
        arraySet[indexSet].darina = arrayGet[indexGet].darina;
        arraySet[indexSet].darinaSum= arrayGet[indexGet].darinaSum;
        arraySet[indexSet].darinaZakup = arrayGet[indexGet].darinaZakup;

        arraySet[indexSet].GornoyKrest = arrayGet[indexGet].GornoyKrest;
        arraySet[indexSet].GornoyKrestSum = arrayGet[indexGet].GornoyKrestSum;
        arraySet[indexSet].GornoyKrestZakup = arrayGet[indexGet].GornoyKrestZakup;

        arraySet[indexSet].Dombay = arrayGet[indexGet].Dombay;
        arraySet[indexSet].DombaySum = arrayGet[indexGet].DombaySum;
        arraySet[indexSet].DombayZakup = arrayGet[indexGet].DombayZakup;
        
        arraySet[indexSet].GornoyVersh = arrayGet[indexGet].GornoyVersh;
        arraySet[indexSet].GornoyVershSum = arrayGet[indexGet].GornoyVershSum;
        arraySet[indexSet].GornoyVershZakup = arrayGet[indexGet].GornoyVershZakup;

        arraySet[indexSet].Pilegrim = arrayGet[indexGet].Pilegrim;
        arraySet[indexSet].PilegrimSum = arrayGet[indexGet].PilegrimSum;
        arraySet[indexSet].PilegrimZakup = arrayGet[indexGet].PilegrimZakup;

        arraySet[indexSet].Kubay = arrayGet[indexGet].Kubay;
        arraySet[indexSet].KubaySum = arrayGet[indexGet].KubaySum;
        arraySet[indexSet].KubayZakup = arrayGet[indexGet].KubayZakup;

        arraySet[indexSet].Tanais = arrayGet[indexGet].Tanais;
        arraySet[indexSet].TanaisSum = arrayGet[indexGet].TanaisSum;
        arraySet[indexSet].TanaisZakup = arrayGet[indexGet].TanaisZakup;

        arraySet[indexSet].WorkPrise = arrayGet[indexGet].WorkPrise;

        arraySet[indexSet].isDebts = arrayGet[indexGet].isDebts;
        arraySet[indexSet].isAlien = arrayGet[indexGet].isAlien;
        arraySet[indexSet].edidObjekt = arrayGet[indexGet].edidObjekt;

        arraySet[indexSet].alpfaform = arrayGet[indexGet].alpfaform;
        if(bufer)arraySet[indexSet].Polatrans = bufer;

        return arraySet[indexSet];
    }

    public void Sort()
    {
       
       for (int i = Kontragent.Count - 1; i >= 0 ; i--)
        {
            Destroy(Kontragent[i].alpfaform.gameObject);
        }

        Transform Pref;
        for (int i = 0; i < Kontragent.Count; i++) 
        {
            Pref=Instantiate(PrefabKontragent);
            Pref.SetParent(SerchMenu.GetChild(0));
           
            Pref.GetChild(0).GetComponent<Text>().text= i.ToString();//копируем порядочный номер в игровой объект
            Kontragent[i].alpfaform = Pref.transform;
            Pref.name = "Кнопка";
            
            Pref.GetChild(1).GetComponent<InputField>().text=Kontragent[i].Adress;
            Kontragent.Sort(SortKontragent);
        }
    }

    int SortKontragent(Items a, Items b)
    {
        
        if(a.Adress != null && b.Adress != null)
        {
            return a.Adress.CompareTo(b.Adress);
        }
            return 0;
    }    

    public void Quit()
    {
        Application.Quit();
    }
    public void QuitOptions()
    {
        transform.parent.Find("Меню Настроек").gameObject.SetActive(false);
    }
    public void CopyTelephoneInArray(Transform MyTransform) //передает телефон в массив
    {
        for (int i = 0; i < polaReis.Count; i++)
        {
            if(polaReis[i].Polatrans.gameObject==MyTransform.gameObject)
            {
                polaReis[i].telephone = MyTransform.GetChild(2).GetComponent<InputField>().text;
            }                
        }

        for (int i = 0; i < polaReis2.Count; i++)
        {
            if(polaReis2[i].Polatrans.gameObject==MyTransform.gameObject)
            {
                polaReis2[i].telephone = MyTransform.GetChild(2).GetComponent<InputField>().text;
            }                
        }
        
    }
}
