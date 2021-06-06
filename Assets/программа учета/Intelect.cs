using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;//позволяет создовать и обращаться к сторонним класам
using System.IO;//позволяет обращаться и создавать  тектовые документы


public class Intelect : MonoBehaviour
{

    #region переменные
    public GUIStyle style;
    private string col;//переменная нужна для сохранения в файл цвета кнопки
    private string saveSTATUS ; //файл для сохранения данных СТАТИСТИКИ
    
    public List<intelect> massive = new List<intelect>();
    public List<intelect> kontragent = new List<intelect>();
    public List<intelect> voda = new List<intelect>();
    public List<intelect> Statis = new List<intelect>();
    public List<intelect> dublekontr = new List<intelect>();
    public List<intelect> rashodOrder = new List<intelect>();

    private Texture loadTexture ;//для загрузки текстуры в кнопку
    
    
    private string primethanie;//примечяания
    
    
    public ItemDefinee bufer;//должен быть всегда паблик
    private int sloy = 0;
    private bool on = false;//переменная сообщает включен ли шифт
    private bool zapros = false;//переменная 
    private int cassa=0;
    private int cassaplus = 0;
    private string textcassa = "";
    private int index;//счетчик цикла маршрутника 
    private int indexx1;//счетчик цикла контрагентов
    private int stat;//счетчик цикла статуса

    private Vector2 ctor;//для скрола контрагентов
    private Vector2 cto=new Vector2(0,0);//для скрола статуса
    private int rashod=0;//в меню статуса 
    private int dohod;//в меню статуса
    private int koldo;//общ кол во на доставку надо для интервалов
    private int prido;//общ прибыль на доставку надо для интервалов
    private int kollo;//общ кол во c ларька для интервалов
    private int dohlo;//общ прибыль c ларька для интервалов

    
    private string textrashod = "";
    private string textdohod = "";
    private string otseevatel = "";
    
    public int indeRa;//счетчик цикла расхода
    
    private bool o = true;
    private int a = 0;
    private int inarenda;//вычет денег у аренды 
    private int invithet;
    private int inrashod;
    private int indohod;

    //блок переменных эксперементальных потом переделать надо
    private int osz = 0;//переменные расчитывают сколько денег надо на закупку товара
    private int perd = 0;// переменная сюда суммируем прибыль за каждый день
    private int s = 0;// переменная сюда суммируем прибыль за каждый день
    private int b = 0;//начальный интервал
    private int c = 0;//конечный интервал
    private int v = 0;//кол во бутылок за интервал
    private int n = 0;//расходы за интервал
    private int ns = 0;//переменная сюда суммируем  расходы за интервал
    private int f = 0;//переменная сюда суммируем  прибыль за интервал от доставки
    private int g = 0;//переменная сюда суммируем  кол во за интервал с ларька
    private int h = 0;//переменная сюда суммируем  прибыль за интервал с ларька
    private int dney = 0;//сколько дней интервала
    public float dosHT;//средняя доставка шт
    private float dosPRI;//средняя доставка при  
    private float larHT;//средняя ларек шт
    private float larPRI;//средняя ларек при
    private float BatDos;//батищев+доставка
    private float BatPRI;//батищев средняя

    #endregion
    void Start()//функция загружает данные из файлов
    {
       
        //    massive.Add(new phonee());//добавление места в массив
        //    kontragent.Add(new phonee());//добавление места в массив
        #region загружаем дени и проверяем можно ли создать новый 
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        StreamReader statusdat = new StreamReader("SaveDat");//загружаем данные статистики

            int.TryParse(statusdat.ReadLine(), out stat);//загружаем цикл stat и переводим текст в число...вродебы
            for (int i = 0; i < stat; i++)
            {
                Statis[i].item.dat = statusdat.ReadLine();//читаем дату (используется для сохранения дня)
                saveSTATUS = Statis[i].item.dat;//подставляем в текстовое поле дату (не является непосредственно структурой statusdat....просто тут удобно это было разместить ) 
            }
        statusdat.Close();
        
        for (int i = 0; i < indexx1; i++)//тут я устанавливаю черный цвет, кнопки выбрать у конторагентов 
        {
            kontragent[i].item.st.normal.textColor = Color.black;
            dublekontr[i].item.st.normal.textColor = Color.black;
        }
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        zapros = true;//по умолчанию даем разрешение на открытие нового дня 
        for (int i = 0; i < stat; i++){if (Statis[i].item.dat == System.DateTime.Now.ToString("dd.MM.yyyy")) { zapros = false; }}//защита от создания нового сегоднешнего дня повторно
        if (zapros == true)//тут мы задаем вопрос можно ли создать новый день
        {
            for (int i = 0; i < 10; i++)// в новом дне продажи с ларька 0 по всем пазициям
            {
                voda[i].item.serproBAY = 0;//после записи в архив обнуляем данные ларька(кол-во бут)
                voda[i].item.serproVAR = 0;//после записи в архив обнуляем данные ларька(сумма)
                voda[i].item.serproPRI = 0;//после записи в архив обнуляем данные ларька(прибыль)
            }
            Statis[stat].item.dat = System.DateTime.Now.ToString("dd.MM.yyyy");//устанавливаем сегоднешнюю дату(нужно для сохранения дня в файл)
            saveSTATUS = Statis[stat].item.dat;//не понял!!зачем я это сделал...? 
            stat++;
            loadTexture = Resources.Load("bleack") as Texture;// в новом дне подставляем текстурку черного цвета
            col = "черный";
            style.normal.textColor = Color.black;
            Status();
            
        }
       //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        for (int i = 0; i < stat; i++)//загружает именно сегоднешний день(нужно для того что бы при запуске программы выбирался зеленым последний день в списке)
        {
            if (Statis[i].item.dat== System.DateTime.Now.ToString("dd.MM.yyyy"))
            {
                Statis[i].item.st.normal.textColor = Color.green;
                saveSTATUS = Statis[i].item.dat;
                loadStatus();
            }
        }
        if (col == "зеленый") { style.normal.textColor = Color.green;}//изменяет цвета кнопки в кассу
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            #endregion
        #region загружаем сохранения ларька
        StreamReader load = new StreamReader("Save");//"C:/SaveUnity3d/Save");//загружает ларек
        int.TryParse(load.ReadLine(), out cassa);//пока не проверенно работает ли   ///надо исправить загружает инфо 10раз
        

        for (int i = 0; i < 10; i++)
        {
            voda[i].item.Name = load.ReadLine();

            int.TryParse(load.ReadLine(), out voda[i].item.serpro);//
          //int.TryParse(load.ReadLine(), out voda[i].item.serproBAY);
            int.TryParse(load.ReadLine(), out voda[i].item.serproMoney);
         // int.TryParse(load.ReadLine(), out voda[i].item.serproVAR);
         // int.TryParse(load.ReadLine(), out voda[i].item.serproPRI);
            int.TryParse(load.ReadLine(), out voda[i].item.serproZakup);
            int.TryParse(load.ReadLine(), out voda[i].item.serproZA);
            int.TryParse(load.ReadLine(), out voda[i].item.serproVARZkup);
            voda[i].item.convert1 = load.ReadLine();
            voda[i].item.convert2 = load.ReadLine();
            voda[i].item.convert3 = load.ReadLine();
            int.TryParse(load.ReadLine(), out voda[i].item.kolDostt);
            
            
        }
        load.Close();

        if (zapros == true)//тут мы добовляем ЗП водителю и расходы на аренду
        {

            primethanie = primethanie +  "="+ cassa + "Касса на утро";//добавляем основные расходы в примечание
           
            Status();
            //не в коем случае не писать сюда Save() Удалит контрагентов
        }
        
           
           
            
            
        
        #endregion
        #region загружаем сохранения Контрагентов
        StreamReader loadDUST = new StreamReader("SaveKontragent");//загружает Контрагентов
        for (int i = 0; i < 200; i++)
        {
            int.TryParse(loadDUST.ReadLine(), out indexx1);
           
                kontragent[i].item.contrname = loadDUST.ReadLine();
                kontragent[i].item.telephone = loadDUST.ReadLine();
                kontragent[i].item.cenaCP= loadDUST.ReadLine();
                kontragent[i].item.cenaDM = loadDUST.ReadLine();
        }
        loadDUST.Close();
        #endregion
        cto = Vector2.positiveInfinity;//метод перемещает скролл статуса в самый низ
    }
    void Update()
    {
        
        transform.Rotate(0, 0.002f, 0.003f);
        //  var actualTime = System.DateTime.Now.ToString("hh:mm:ss"); время!!!!месяц год день
        //  var date = System.DateTime.Now.ToString("MM/dd/yyyy");
        //  month = System.DateTime.Now.get_Month();
        //  day = System.DateTime.Now.get_Day();
        //  year = System.DateTime.Now.get_Year();
        if (Input.GetKeyDown(KeyCode.LeftShift)){ on = true; }//если включена то +10
        if (Input.GetKeyUp(KeyCode.LeftShift)) { on = false;  }//если выключяена то +1
        if (Input.GetKeyUp(KeyCode.Mouse1)){if (sloy == 0) { sloy = 2; }else sloy = 0;}

    }//тут только проверка на нажатие клавиш
    void Save()
    {
        StreamWriter sw = new StreamWriter("Save"); //файл для сохранения данных ЛАРЬКА
        sw.WriteLine(cassa);//пока не проверенно работает ли
        for (int i = 0; i < 10; i++)
        {
            sw.WriteLine(voda[i].item.Name);
            sw.WriteLine(voda[i].item.serpro);
         //   sw.WriteLine(voda[i].item.serproBAY);
            sw.WriteLine(voda[i].item.serproMoney);
         //   sw.WriteLine(voda[i].item.serproVAR);
        //    sw.WriteLine(voda[i].item.serproPRI);
            sw.WriteLine(voda[i].item.serproZakup);
            sw.WriteLine(voda[i].item.serproZA);
            sw.WriteLine(voda[i].item.serproVARZkup);
            sw.WriteLine(voda[i].item.convert1);
            sw.WriteLine(voda[i].item.convert2);
            sw.WriteLine(voda[i].item.convert3);
            sw.WriteLine(voda[i].item.kolDostt);
            
            
        }

        sw.Close();
        StreamWriter sdust = new StreamWriter("SaveKontragent");//файл для сохранения данных КОНТРАГЕНТОВ
        for (int i = 0; i < 200; i++)
        {
            
            sdust.WriteLine(indexx1);
            sdust.WriteLine(kontragent[i].item.contrname);
            sdust.WriteLine(kontragent[i].item.telephone);
            sdust.WriteLine(kontragent[i].item.cenaCP);
            sdust.WriteLine(kontragent[i].item.cenaDM);
            
        }
        sdust.Close();
      
    }//функция сохраняет контрагентов,маршрутный лист,и данные ларька
    void OnGUI( )
    {
        
        int viruthDost = 0;//общая сумма выручки                        //
        int pribDost = 0;//общая сумма  прибыли каждой позиции          // счетчики доставки 3 бокса в самом низу
        int kolDost = 0;//общая cyмма проданных бутылок каждой позиции  //
        int cpob = 0;//общая cyмма сп на доставку                       //
        int dmob = 0;//общая cyмма дм на доставку                       //


        int doh = 0;//общая сумма  прибыли каждой позиции               //
        int sum = 0;//общая cyмма проданных бутылок каждой позиции      // счетчики ларька 3 бокса в самом низу
        int VAR = 0;//общая сумма выручки                               //
        int ZAK = 0;//общая сумма денег на закупку                      //
        int ko = 0;                                         // 
                                             
        
        #region доставка слой 0
        if (sloy == 0)
        {
            
            GUI.Box(new Rect(10, 0, 640, 745), "Панель Доставки");
            GUI.Box(new Rect(20, 70, 620, 25), "                           Адрес                              телефон      СП     цена     ДМ     цена     выруч            ");
            for (int i = 0; i < index; i++)
            {
                GUI.Box(new Rect(10,(i * 30) + 100, 15, 25), i+1 + "");
                GUI.Box(new Rect(26,(i * 30) + 100, 250, 25),massive[i].item.contrname+ "");//адрес
                GUI.Box(new Rect(280,(i * 30) + 100, 85, 25), massive[i].item.telephone+"");//телефон

             //   massive[i].item.CECPSTR = GUI.TextArea(new Rect(367,(i * 30) + 100, 30, 25),massive[i].item.CECPSTR, 3);//кол-во СП
              //  int.TryParse(massive[i].item.CECPSTR, out massive[i].item.CECP);//конвертация текста в число
                GUI.Box(new Rect(367,(i * 30) + 100, 30, 25), massive[i].item.CECP + "");//кол-во сп

                GUI.Box(new Rect(400,(i * 30) + 100, 50, 25), massive[i].item.CPCP + " руб");//цена СП
                int.TryParse(massive[i].item.cenaCP, out massive[i].item.CPCP);//конвертация текста в число

                 GUI.Box(new Rect(453,(i * 30) + 100, 30, 25), massive[i].item.CEDM + "");//кол-во дм
             //   massive[i].item.CEDMSTR = GUI.TextArea(new Rect(453,(i * 30) + 100, 30, 25), massive[i].item.CEDMSTR, 3);//кол-во дм
              //  int.TryParse(massive[i].item.CEDMSTR, out massive[i].item.CEDM);//конвертация текста в число

                GUI.Box(new Rect(487,(i * 30) + 100, 50, 25), massive[i].item.CPDM + " руб");//цена ДМ
                int.TryParse(massive[i].item.cenaDM, out massive[i].item.CPDM);//конвертация текста в число

                GUI.Box(new Rect(540,(i * 30) + 100, 50, 25), massive[i].item.CECP * massive[i].item.CPCP+ massive[i].item.CEDM* massive[i].item.CPDM + "");//выручка
                #region кнопки +- от которых отказались
                /*        if (GUI.Button(new Rect(394,(i * 30) + 100, 30, 15), "+")&& voda[0].item.serpro>0) {
                            massive[i].item.CECP++;voda[0].item.serpro--;
                            viruthDost = 0; massive[i].item.VIR = (massive[i].item.CECP * massive[i].item.CPCP) + (massive[i].item.CEDM * massive[i].item.CPDM);
                            Save();
                         }

                        if (GUI.Button(new Rect(394,(i * 30) + 115, 30, 10), "-")&& massive[i].item.CECP>0)
                        {
                            massive[i].item.CECP--; voda[0].item.serpro++;
                            massive[i].item.VIR = 0; massive[i].item.VIR = (massive[i].item.CECP * massive[i].item.CPCP) + (massive[i].item.CEDM * massive[i].item.CPDM);
                            viruthDost = viruthDost + massive[i].item.VIR;
                            Save();
                        }

                        if (GUI.Button(new Rect(506,(i * 30) + 100, 30, 15), "+")&& voda[1].item.serpro>0)
                        {
                            massive[i].item.CEDM++; voda[1].item.serpro--; massive[i].item.VIR = 0;
                            massive[i].item.VIR = (massive[i].item.CECP * massive[i].item.CPCP) + (massive[i].item.CEDM * massive[i].item.CPDM);
                            viruthDost = viruthDost + massive[i].item.VIR;
                            Save();
                        }

                        if (GUI.Button(new Rect(506,(i * 30) + 115, 30, 10), "-")&& massive[i].item.CEDM>0)
                        {
                            massive[i].item.CEDM--; voda[1].item.serpro++; massive[i].item.VIR = 0;
                            massive[i].item.VIR = (massive[i].item.CECP * massive[i].item.CPCP) + (massive[i].item.CEDM * massive[i].item.CPDM);
                            viruthDost = viruthDost + massive[i].item.VIR;
                            Save();
                        }*/
                #endregion


                cpob = cpob + massive[i].item.CECP;
                dmob = dmob + massive[i].item.CEDM;
                kolDost = kolDost + (massive[i].item.CECP+ massive[i].item.CEDM);//счетчик кол-во доставки
                viruthDost = viruthDost + (massive[i].item.CECP * massive[i].item.CPCP) + (massive[i].item.CEDM * massive[i].item.CPDM);
                // viruthDost = viruthDost + massive[i].item.VIR;//счетчик выручки доставки (отказались от метода..неудобно)
                pribDost = pribDost + (massive[i].item.CECP * (massive[i].item.CPCP - voda[0].item.serproZakup))+(massive[i].item.CEDM * (massive[i].item.CPDM-voda[1].item.serproZakup));//счетчик прибыли доставки
                if (GUI.Button(new Rect(595,(i * 30) + 100, 40, 25), "X"))
                {
                    if (col == "черный")
                    {
                        index--;
                        voda[0].item.serpro = voda[0].item.serpro + massive[i].item.CECP;
                        voda[1].item.serpro = voda[1].item.serpro + massive[i].item.CEDM;
                        massive.RemoveAt(i);
                        Status();
                    }
                }
                

            }
            GUI.Box(new Rect(367, (index * 30) + 100,116, 25),  cpob+ "                  "+ dmob);//счетчик общ кол-во СП
            GUI.Box(new Rect(170, 640, 100, 90), "Общ-кол: \n\n" + kolDost);//общ кол-во доставки
            GUI.Box(new Rect(290, 640, 100, 90), "Выручка: \n\n" + viruthDost);//общ выручка доставки
            GUI.Box(new Rect(410, 640, 100, 90), "Прибыль: \n\n" + pribDost);//общ прибыль доставки
            koldo = kolDost;
            prido = pribDost;


        }
        #endregion
        #region лист контрагентов слой 1 
        if (sloy == 1)
        {
            
            GUI.Box(new Rect(10,0,1640,745), "Панель контрагентов   ");

            GUI.Box(new Rect(30,20,1600,25), "");//дизайнерское решение
            GUI.Box(new Rect(30,690,1600,25), "");//дизайнерское решение
            if (GUI.Button(new Rect(750,60,150,100), "Создать контрагента"))
            {
                indexx1++;
            }
            otseevatel = GUI.TextArea(new Rect(750, 300, 150, 50), otseevatel, 200);
            ctor =  GUI.BeginScrollView(new Rect(0, 50, 680, 635), ctor, new Rect(0,45,0,6000));
            
                #region  список контрагентов
                for (int i = 0; i < indexx1; i++)
            {
                if (i == 0)                                     //
                {                                               //
                    for (int x = 0; x < 200; x++)               //
                    {                                           //конструкция нужна для обновления второго массива контрагентов 
                        dublekontr[x].item.contrname = null;
                        dublekontr[x].item.telephone = null;
                        dublekontr[x].item.cenaCP = null;
                        dublekontr[x].item.cenaDM = null;
                        
                        
                        a = 0;                                  //
                    }                                           //
                }                                               //
                if (kontragent[i].item.contrname.Contains(otseevatel))//метод отсеевателя по названию

                {

                    if (otseevatel != "") { Otseel(kontragent[i].item);ctor = Vector2.zero; }//передача информации вотрому массиву


                    if (otseevatel == "")
                    {

                        GUI.Box(new Rect(90, (i * 30) + 50, 35, 25), i + "");
                        kontragent[i].item.contrname = GUI.TextField(new Rect(130, (i * 30) + 50, 250, 25), kontragent[i].item.contrname, 200);
                        kontragent[i].item.telephone = GUI.TextArea(new Rect(380, (i * 30) + 50, 85, 25), kontragent[i].item.telephone, 11);
                        kontragent[i].item.cenaCP = GUI.TextArea(new Rect(470, (i * 30) + 50, 30, 25), kontragent[i].item.cenaCP, 3);
                        kontragent[i].item.cenaDM = GUI.TextArea(new Rect(500, (i * 30) + 50, 30, 25), kontragent[i].item.cenaDM, 3);

                        kontragent[i].item.CECPSTR = GUI.TextField(new Rect(600, (i * 30) + 50, 30, 25), kontragent[i].item.CECPSTR, 3);
                        int.TryParse(kontragent[i].item.CECPSTR, out kontragent[i].item.CECP);//конвертация текста в число
                        kontragent[i].item.CEDMSTR = GUI.TextField(new Rect(630, (i * 30) + 50, 30, 25), kontragent[i].item.CEDMSTR, 3);
                        int.TryParse(kontragent[i].item.CEDMSTR, out kontragent[i].item.CEDM);//конвертация текста в число

                        if (kontragent[i].item.CECP > 0 && col == "черный" || kontragent[i].item.CEDM > 0 &&col== "черный")
                        {
                            if (GUI.Button(new Rect(535, (i * 30) + 50, 60, 25), "Выбрать", kontragent[i].item.st))
                            {
                                index++;
                                kontragent[i].item.st.normal.textColor = Color.green;
                                massive[index - 1].item.contrname = kontragent[i].item.contrname;
                                massive[index - 1].item.telephone = kontragent[i].item.telephone;
                                massive[index - 1].item.cenaCP = kontragent[i].item.cenaCP;
                                massive[index - 1].item.cenaDM = kontragent[i].item.cenaDM;
                                massive[index - 1].item.CECP = kontragent[i].item.CECP;
                                massive[index - 1].item.CEDM = kontragent[i].item.CEDM;
                                voda[0].item.serpro = voda[0].item.serpro - kontragent[i].item.CECP;
                                voda[1].item.serpro = voda[1].item.serpro - kontragent[i].item.CEDM;
                                kontragent[i].item.CECPSTR = "";
                                kontragent[i].item.CEDMSTR = "";
                                Status();
                               
                            }
                        }
                    }
                }
            }
            
            #endregion
            #region отсееватель
            if (otseevatel != "")
            {
                for (int i = 0; i < 21; i++)
                {
                    GUI.Box(new Rect(90, (i * 30) + 50, 35, 25), i + "");
                    GUI.Box(new Rect(130, (i * 30) + 50, 250, 25), dublekontr[i].item.contrname + "");
                    GUI.Box(new Rect(380, (i * 30) + 50, 85, 25), dublekontr[i].item.telephone + "");
                    GUI.Box(new Rect(470, (i * 30) + 50, 30, 25), dublekontr[i].item.cenaCP + "");
                    GUI.Box(new Rect(500, (i * 30) + 50, 30, 25), dublekontr[i].item.cenaDM + "");

                    dublekontr[i].item.CECPSTR = GUI.TextArea(new Rect(600, (i * 30) + 50, 30, 25), dublekontr[i].item.CECPSTR, 3);
                    int.TryParse(dublekontr[i].item.CECPSTR, out dublekontr[i].item.CECP);//конвертация текста в число
                    dublekontr[i].item.CEDMSTR = GUI.TextArea(new Rect(630, (i * 30) + 50, 30, 25), dublekontr[i].item.CEDMSTR, 3);
                    int.TryParse(dublekontr[i].item.CEDMSTR, out dublekontr[i].item.CEDM);//конвертация текста в число
                    
                    if (dublekontr[i].item.CECP > 0 || dublekontr[i].item.CEDM > 0)
                    {
                       
                        if (GUI.Button(new Rect(535, (i * 30) + 50, 65, 25), "Выбрать", dublekontr[i].item.st))
                        {
                            
                            index++;
                            dublekontr[i].item.st.normal.textColor = Color.green;
                            massive[index - 1].item.contrname = dublekontr[i].item.contrname;
                            massive[index - 1].item.telephone = dublekontr[i].item.telephone;
                            massive[index - 1].item.cenaCP = dublekontr[i].item.cenaCP;
                            massive[index - 1].item.cenaDM = dublekontr[i].item.cenaDM;
                            massive[index - 1].item.CECP = dublekontr[i].item.CECP;
                            massive[index - 1].item.CEDM = dublekontr[i].item.CEDM;
                            voda[0].item.serpro = voda[0].item.serpro - dublekontr[i].item.CECP;
                            voda[1].item.serpro = voda[1].item.serpro - dublekontr[i].item.CEDM;
                            dublekontr[i].item.CECPSTR = "";
                            dublekontr[i].item.CEDMSTR = "";
                            Status();
                        }
                    }
                }
            }
            #endregion
            GUI.EndScrollView();
            
            #region дублирую маршрутник
            for (int i = 0; i < index; i++)
            {
                GUI.Box(new Rect(950, (i * 30) + 55, 25, 25), i + 1 + "");
                GUI.Box(new Rect(976, (i * 30) + 55, 250, 25), massive[i].item.contrname + "");//адрес
                GUI.Box(new Rect(1230, (i * 30) + 55, 85, 25), massive[i].item.telephone + "");//телефон


                GUI.Box(new Rect(1317, (i * 30) + 55, 30, 25), massive[i].item.CECP + "");//кол-во сп

                GUI.Box(new Rect(1350, (i * 30) + 55, 50, 25), massive[i].item.CPCP + " руб");//цена СП
                int.TryParse(massive[i].item.cenaCP, out massive[i].item.CPCP);//конвертация текста в число

                GUI.Box(new Rect(1403, (i * 30) + 55, 30, 25), massive[i].item.CEDM + "");//кол-во дм

                GUI.Box(new Rect(1437, (i * 30) + 55, 50, 25), massive[i].item.CPDM + " руб");//цена ДМ
                int.TryParse(massive[i].item.cenaDM, out massive[i].item.CPDM);//конвертация текста в число

                GUI.Box(new Rect(1490, (i * 30) + 55, 50, 25), massive[i].item.CECP * massive[i].item.CPCP + massive[i].item.CEDM * massive[i].item.CPDM + "");//выручка

                cpob = cpob + massive[i].item.CECP;
                dmob = dmob + massive[i].item.CEDM;
                kolDost = kolDost + (massive[i].item.CECP + massive[i].item.CEDM);//счетчик кол-во доставки
                viruthDost = viruthDost + (massive[i].item.CECP * massive[i].item.CPCP) + (massive[i].item.CEDM * massive[i].item.CPDM);

                pribDost = pribDost + (massive[i].item.CECP * (massive[i].item.CPCP - voda[0].item.serproZakup)) + (massive[i].item.CEDM * (massive[i].item.CPDM - voda[1].item.serproZakup));//счетчик прибыли доставки
                if (GUI.Button(new Rect(1545, (i * 30) + 55, 40, 25), "X"))
                {
                    index--;
                    voda[0].item.serpro = voda[0].item.serpro + massive[i].item.CECP;
                    voda[1].item.serpro = voda[1].item.serpro + massive[i].item.CEDM;
                    massive.RemoveAt(i);
                    Status();
                }
                
            }
            GUI.Box(new Rect(1317, (index * 30) + 55, 116, 25), cpob + "                  " + dmob);//счетчик общ кол-во СП
            #endregion
            
        }
        #endregion
        #region лист ларька слой 0
        if (sloy == 0)
        {
            #region Статус 
            GUI.Box(new Rect(1300, 0, 350, 745), "Статистика");
            GUI.Box(new Rect(1310,70, 328, 25), "");
            cto = GUI.BeginScrollView(new Rect(90, 100, 1550, 630), cto, new Rect(100, 100, 0, 270+stat*23));
            for (int i = 0; i < stat; i++)
            {
                GUI.Box(new Rect(1320,(i * 30) + 100, 100, 25), Statis[i].item.dat + "");//дата
                if (GUI.Button(new Rect(1440,(i * 30) + 100, 110, 25), "Загрузить день", Statis[i].item.st))
                {
                    
                    for (int a = 0; a < stat; a++) { Statis[a].item.st.normal.textColor = Color.black; }//выключает цвет кнопки
                    Statis[i].item.st.normal.textColor = Color.green;
                    saveSTATUS = Statis[i].item.dat;
                    for (int v = 0; v < indeRa; v++)
                    {
                        rashodOrder[v].item.Name = "";
                        rashodOrder[v].item.dat = "";
                        rashodOrder[v].item.VIR = 0;
                        rashodOrder[v].item.CPCP = 0;
                    }
                    
                    loadStatus();
                    if (col == "зеленый") { style.normal.textColor = Color.green; }
                    if (col == "черный") { style.normal.textColor = Color.black; }
                }
                if (GUI.Button(new Rect(1570,(i * 30) + 100, 45, 25), "X"))
                {
                }
            }
            GUI.EndScrollView();
            #endregion

            GUI.Box(new Rect(650, 0, 650, 745), "Склад");
            GUI.Box(new Rect(925, 25, 100, 25), "" + System.DateTime.Now.ToString("dd.MM.yyyy"));//дата в ларьке
            GUI.Box(new Rect(660, 70, 630, 25), "название        остаток   продано   цена       выручка      прибыль                          ");
            for (int i = 0; i < 10; i++)
            {

                
                GUI.Box(new Rect(665,(i * 30) +100, 150, 25), voda[i].item.Name + "");//строка названия воды
                GUI.Box(new Rect(820,(i * 30) + 100, 50, 25), voda[i].item.serpro + " шт");//получаем кол-во на складе
                GUI.Box(new Rect(875,(i * 30) + 100, 40, 25), voda[i].item.serproBAY + " шт");//получаем кол-во проданого
                GUI.Box(new Rect(920,(i * 30) + 100, 55, 25), voda[i].item.serproMoney + " руб");//получаем  цену
                GUI.Box(new Rect(980,(i * 30) + 100, 75, 25), voda[i].item.serproVAR + " руб");//получаем  выручку
                GUI.Box(new Rect(1060,(i * 30) + 100, 75, 25), voda[i].item.serproPRI + " руб");//получаем  прибыль
                if (GUI.Button(new Rect(1145,(i * 30) + 100, 70, 25), "продать") && voda[i].item.serpro > 0)
                {
                    
                    if (on == true&& voda[i].item.serpro >= 10)
                    {
                        voda[i].item.serpro = voda[i].item.serpro - 10; voda[i].item.serproBAY = voda[i].item.serproBAY + 10;//счетчик кол-во бутылок продоваемых
                        voda[i].item.serproVAR = voda[i].item.serproBAY * voda[i].item.serproMoney;//сумма выручки
                        voda[i].item.serproPRI = (voda[i].item.serproMoney - voda[i].item.serproZakup) * voda[i].item.serproBAY;//счетчик прибыли
                        cassa = cassa + (voda[i].item.serproMoney * 10);
                    }
                    if (on == false )
                    {
                        voda[i].item.serpro = voda[i].item.serpro - 1; voda[i].item.serproBAY = voda[i].item.serproBAY + 1;//счетчик кол-во бутылок продоваемых
                        voda[i].item.serproVAR = voda[i].item.serproBAY * voda[i].item.serproMoney;//сумма выручки
                        voda[i].item.serproPRI = (voda[i].item.serproMoney - voda[i].item.serproZakup) * voda[i].item.serproBAY;//счетчик прибыли
                        cassa = cassa + voda[i].item.serproMoney;
                    }
                    Save();
                    Status();
                }

                doh = doh + voda[i].item.serproPRI;//счетчик общей прибыли
                VAR = VAR + voda[i].item.serproVAR;//счетчик общей выручки
                sum = sum+voda[i].item.serproBAY;//счетчик общих кол-ва проданных бутылок
                
                bufer.dat = System.DateTime.Now.ToString("dd.MM.yyyy");

               
                if (GUI.Button(new Rect(1220,(i * 30) + 100, 70, 25), "вернуть") && voda[i].item.serproBAY > 0)
                {
                    if (on == true && voda[i].item.serproBAY >= 10)
                    {
                        voda[i].item.serpro = voda[i].item.serpro + 10; voda[i].item.serproBAY = voda[i].item.serproBAY - 10;
                        voda[i].item.serproVAR = voda[i].item.serproBAY * voda[i].item.serproMoney;
                        voda[i].item.serproPRI = (voda[i].item.serproMoney - voda[i].item.serproZakup) * voda[i].item.serproBAY;
                        cassa = cassa - (voda[i].item.serproMoney * 10);
                    }
                    if (on == false )
                    {
                        voda[i].item.serpro = voda[i].item.serpro + 1; voda[i].item.serproBAY = voda[i].item.serproBAY - 1;
                        voda[i].item.serproVAR = voda[i].item.serproBAY * voda[i].item.serproMoney;
                        voda[i].item.serproPRI = (voda[i].item.serproMoney - voda[i].item.serproZakup) * voda[i].item.serproBAY;
                        cassa = cassa - voda[i].item.serproMoney;
                    }
                    Save();
                    Status();
                }
                

            }
           
            GUI.Box(new Rect(810, 640, 100, 90), " Общ кол-во: \n\n" + sum);//кол-во проданных бутылок
            GUI.Box(new Rect(940, 640, 120, 90), "Выручка сегодня: \n\n" + VAR);//выручка
            GUI.Box(new Rect(1085, 640,120, 90), "Прибыль сегодня: \n\n" + doh);//прибыль
            kollo = sum;
            dohlo = doh;
            
        }if(sloy==0||sloy==3) GUI.Box(new Rect(675, 640, 100, 90), "Касса: \n\n"+cassa);//иконка кассы
        #endregion
        #region лист закупки слой 3
        if (sloy == 3)
        {
            osz = 0;
            
            GUI.Box(new Rect(10, 0, 1640, 745), "Закупка воды");
            GUI.Box(new Rect(30, 20, 1600, 25), "");//дизайнерское решение
            GUI.Box(new Rect(30, 690, 1600, 25), "");//дизайнерское решение
            GUI.Box(new Rect(465, 70, 790, 25), "Название           Остаток  Кол-во      Цена        Сумма                                                                                       ");
            for (int i = 0; i < 10; i++)
            {
                
                ZAK = (voda[i].item.kolDostt - voda[i].item.serpro) * voda[i].item.serproZakup;//расчет денег на закупку товара
                ko = voda[i].item.kolDostt - voda[i].item.serpro;
                osz = osz + ZAK;

                GUI.Box(new Rect(470,(i * 30) + 100, 150, 25), voda[i].item.Name+ "");//название 
                GUI.Box(new Rect(625,(i * 30) + 100, 50, 25), voda[i].item.serpro + " шт");//получаем кол-во на складе
                GUI.Box(new Rect(680,(i * 30) + 100, 50, 25), voda[i].item.serproZA + " шт");//получаем кол-во купленного товара
                GUI.Box(new Rect(735,(i * 30) + 100, 55, 25), voda[i].item.serproZakup + " руб");//получаем  цену закупки
                GUI.Box(new Rect(795,(i * 30) + 100, 70, 25), voda[i].item.serproVARZkup + " руб");//получаем  сумму закупа

               
                GUI.Box(new Rect(1105,(i * 30) + 100, 60, 25), ZAK + " руб");//получаем  сумму закупа
                GUI.Box(new Rect(1042,(i * 30) + 100, 60, 25), ko + " ");//получаем  кол закупа
                if (GUI.Button(new Rect(880,(i * 30) + 100, 70, 25), "купить"))
                {

                    if (on == true&&cassa>= (voda[i].item.serproZakup * 10)) { voda[i].item.serproZA = voda[i].item.serproZA + 10; voda[i].item.serproVARZkup = voda[i].item.serproZA * voda[i].item.serproZakup; cassa = cassa - (voda[i].item.serproZakup * 10); }
                    if (on == false&&cassa>= voda[i].item.serproZakup) { voda[i].item.serproZA = voda[i].item.serproZA + 1; voda[i].item.serproVARZkup = voda[i].item.serproZA * voda[i].item.serproZakup; cassa = cassa - voda[i].item.serproZakup; }
              
                    Save();



                }
                if (GUI.Button(new Rect(960,(i * 30) + 100, 70, 25), "вернуть"))
                {
                    if (on == true) { voda[i].item.serproZA = voda[i].item.serproZA - 10; voda[i].item.serproVARZkup = voda[i].item.serproZA * voda[i].item.serproZakup;cassa = cassa + (voda[i].item.serproZakup * 10); }
                    if (on == false ) { voda[i].item.serproZA = voda[i].item.serproZA - 1; voda[i].item.serproVARZkup = voda[i].item.serproZA * voda[i].item.serproZakup; cassa = cassa + voda[i].item.serproZakup; }
                    Save();
                }
                if (GUI.Button(new Rect(1180, (i * 30) + 100, 70, 25), "списать"))
                {
                    voda[i].item.serproZA = voda[i].item.serproZA - 1;
                    Save();
                }
            }
            GUI.Box(new Rect(1105, 420, 65, 25), osz + " руб");//получаем общую сумму закупа для общ кол-во
        }
        #endregion
        #region лист номенклатура слой 4
        if (sloy == 4)
        {

            GUI.Box(new Rect(10, 0, 1640, 745), "номенклатура");
            GUI.Box(new Rect(30, 20, 1600, 25), "");//дизайнерское решение
            GUI.Box(new Rect(30, 690, 1600, 25), "");//дизайнерское решение
            GUI.Box(new Rect(515, 70, 620, 25), "Название         Цена закупочная                Цена розничная                               ");
            for (int i = 0; i < 10; i++)
            {
               
                voda[i].item.Name = GUI.TextArea(new Rect(520,(i * 30) + 100, 150, 25), voda[i].item.Name, 200);//название
                voda[i].item.convert1 = GUI.TextArea(new Rect(710,(i * 30) + 100, 50, 25), voda[i].item.convert1, 200);//цена закупочная
                int.TryParse(voda[i].item.convert1, out voda[i].item.serproZakup);//конвертация текста в число

                voda[i].item.convert2 = GUI.TextArea(new Rect(870,(i * 30) + 100, 50, 25), voda[i].item.convert2, 200);//цена закупочная
                int.TryParse(voda[i].item.convert2, out voda[i].item.serproMoney);//конвертация текста в число

                voda[i].item.convert3 = GUI.TextArea(new Rect(970,(i * 30) + 100, 50, 25), voda[i].item.convert3, 200);//цена закупочная
                int.TryParse(voda[i].item.convert3, out voda[i].item.kolDostt);//конвертация текста в число
            }
            
        }
        #endregion
        #region информация слой 2
        if (sloy == 5) //слой интервалов
        {
            int x = 0;
            int y = 0;
            for (int i = 0; i < stat; i++)
            {
                if (x < 1200) { x = x + 110; } else { x = 110; y = y + 30; }

                if (GUI.Button(new Rect(x, y + 100, 110, 25), Statis[i].item.dat + ""))
                { b = i; sloy = 2; }
            }
        }
        if (sloy == 6) //слой интервалов
        {
            int x = 0;
            int y = 0;
            for (int i = 0; i < stat; i++)
            {
                if (x < 1200) { x = x + 110; } else { x = 110; y = y + 30; }

                if (GUI.Button(new Rect(x, y + 100, 110, 25), Statis[i].item.dat + ""))
                { c = i; sloy = 2; }
            }
        }
        if (sloy == 2)
        {
            GUI.Label(new Rect(100, 430, 50, 90), ">>",style);
            GUI.Box(new Rect(30, 20, 1600, 25), "");//дизайнерское решение
            GUI.Box(new Rect(30, 690, 1600, 25), "");//дизайнерское решение
            
           
            GUI.Box(new Rect(10, 0, 1640, 745), "Панель расчетов");

            if (GUI.Button(new Rect(960, 50, 100, 50), "от "+ Statis[b].item.dat))
            {
                sloy = 5;
            }

            if (GUI.Button(new Rect(1070, 50, 100, 50), "до "+ Statis[c].item.dat))
            {
                sloy = 6;
            }
            if (GUI.Button(new Rect(1180, 50, 100, 50), "расчет" ))
            {
                blabla();
                
            }
            GUI.Box(new Rect(1070, 105, 100, 90),"Батищев:\n\n"+s);//батищев за интервал
            GUI.Box(new Rect(1070, 450, 200, 100), "Общие расходы :\n\n" + ns + " руб");//расходы за интервал
            GUI.Box(new Rect(960, 200, 100, 90), "доставка (шт):\n\n"+v + " шт");//кол во бутылок за интервал
            GUI.Box(new Rect(1070, 200, 100, 90), "доставка (при):\n\n" + f + " руб");//кол во бутылок за интервал
            GUI.Box(new Rect(960, 300, 100, 90), "ларек (шт):\n\n" + g + " шт");//кол во бутылок за интервал
            GUI.Box(new Rect(1070, 300, 100, 90), "ларек (при):\n\n" + h + " руб");//ghb,skm за интервал
            GUI.Box(new Rect(1280, 450, 150, 90), "Батищев + Доставка :\n\n" + BatDos + " руб");
            GUI.Box(new Rect(1350, 105, 160, 90), "средняя (при) за" + dney + " дней:\n\n" + BatPRI+" руб");//Батищев
            GUI.Box(new Rect(1180, 200, 160, 90), "средняя (шт) за"+dney + " дней:\n\n" + dosHT + " шт");//доставка
            GUI.Box(new Rect(1350, 200, 160, 90), "средняя (при) за" + dney + " дней:\n\n" + dosPRI + " руб");//доставка
            GUI.Box(new Rect(1180, 300, 160, 90), "средняя (шт) за" + dney + " дней:\n\n" + larHT + " шт");//ларек
            GUI.Box(new Rect(1350, 300, 160, 90), "средняя (при) за" + dney + " дней:\n\n" + larPRI + " руб");//ларек

            textrashod = GUI.TextArea(new Rect(50, 340, 100, 25), textrashod, 20);//статус доп расходы
            if (GUI.Button(new Rect(50, 375, 100, 25), "Расчет"))//расход
            {  
                int.TryParse(textrashod, out inrashod); 
                cassa = cassa - inrashod;
                rashod = rashod+ inrashod;
                
                primethanie=primethanie+ "-"+ textrashod + ":";
                textrashod = "";
                Status();
                Save();
            }

            GUI.Box(new Rect(175, 575, 100, 90), "Прочий доход:\n\n" + dohod);
            textdohod = GUI.TextArea(new Rect(50, 625, 100, 25), textdohod, 20);//статус доп доходы
            if (GUI.Button(new Rect(50, 590, 100, 25), "Расчет"))//доход
            {
                int.TryParse(textdohod, out indohod);
                dohod = dohod + indohod;
                
               // primethanie =primethanie+ "+("+textdohod + ":)";
                textdohod = "";
                Status();
                Save();
            }
            if (GUI.Button(new Rect(175, 525, 100, 40), loadTexture))//кнопка стрелака вверх относится к доходу
            {
                if (loadTexture == Resources.Load("bleack") as Texture)
                {
                    loadTexture = Resources.Load("green") as Texture;
                    cassa = cassa + dohod;
                    
                }
                else
                {
                    loadTexture = Resources.Load("bleack") as Texture;
                    cassa = cassa - dohod;
                    
                }
                Status();
                Save();
            }

            GUI.Box(new Rect(175, 430, 100, 90), "Касса: \n\n" + cassa);//иконка кассы
            GUI.Label(new Rect(280, 460, 100, 90), " --  ");
                    
            GUI.Box(new Rect(175, 320, 100, 90), "Расходы: \n\n" + rashod);//расходы
            GUI.Label(new Rect(215, 410, 100, 90), "  --  ");
            GUI.Box(new Rect(300, 430, 100, 90), "Закуп:\n\n" + osz);//получаем общую сумму закупа для общ кол-во


           
           
            primethanie = GUI.TextArea(new Rect(300, 525, 490,140), primethanie);//окно примечания
            #region новая системма расходов,доходов                                          
            
            GUI.Box(new Rect(50, 50, 600, 250), "");//Дизайнерское решение
            if (GUI.Button(new Rect(55, 55, 70, 25), "Ордер"))
            {
                if (indeRa < 8)
                {
                    indeRa++;
                    Status();
                    Save();

                }

            }
            
            ////textrashod = GUI.TextArea(new Rect(50, 340, 100, 25), textrashod, 20);//статус доп расходы
            for (int i = 0; i < indeRa; i++)
            {
                if (rashodOrder[i].item.VIR == 0)
                {
                    rashodOrder[i].item.dat = GUI.TextArea(new Rect(150, (i * 30) + 55, 100, 25), rashodOrder[i].item.dat, 200);
                    int.TryParse(rashodOrder[i].item.dat, out rashodOrder[i].item.CPCP);
                    rashodOrder[i].item.Name = GUI.TextArea(new Rect(270, (i * 30) + 55, 300, 25), rashodOrder[i].item.Name, 200);
                    
                    if (GUI.Button(new Rect(575, (i * 30) + 55, 25, 25), "ok"))
                    {
                        rashodOrder[i].item.VIR = 1;
                        int.TryParse(rashodOrder[i].item.dat, out inrashod);
                        cassa = cassa - rashodOrder[i].item.CPCP;
                        rashod = rashod + rashodOrder[i].item.CPCP;
                        Status();
                        Save();
                    }
                   
                    if (GUI.Button(new Rect(605, (i * 30) + 55, 25, 25), "X"))
                    {
                        indeRa--;
                        rashodOrder.RemoveAt(i);
                        rashodOrder.Add(new intelect());
                        Status();
                        Save();
                    }
                }
               
                if (rashodOrder[i].item.VIR == 1)
                {
                    GUI.Box(new Rect(150, (i * 30) + 55, 100, 25), rashodOrder[i].item.dat);
                    GUI.Box(new Rect(270, (i * 30) + 55, 300, 25), rashodOrder[i].item.Name);
                    if (GUI.Button(new Rect(575, (i * 30) + 55, 70, 25), "Изменить"))
                    {
                        rashodOrder[i].item.VIR = 0;
                        cassa = cassa + rashodOrder[i].item.CPCP;
                        rashod = rashod - rashodOrder[i].item.CPCP;
                        Status();
                        Save();

                    }
                }
                #endregion
            }
        }
        
        #endregion
        #region кнопки

        if (GUI.Button(new Rect(350, 750, 125, 100), "Закупить")){sloy = 3;}

        if ( sloy == 0 && GUI.Button(new Rect(520, 660, 100, 40), "в кассу >>", style))
        {
            
            if (style.normal.textColor != Color.green)
            {
                
                cassa = cassa + viruthDost;
                style.normal.textColor = Color.green;
                col = "зеленый"; 
                
            }
            else
            {
                cassa = cassa - viruthDost;
                style.normal.textColor = Color.black;
                col = "черный";
            }
            Status();
            Save();
        }
        
        if (GUI.Button(new Rect(10, 750, 125, 100), "Номенклатура")) { sloy = 4; Save(); }

        if (GUI.Button(new Rect(180, 750, 125, 100), "Контрагенты")) { sloy = 1; Save(); }

        if (sloy == 0 && GUI.Button(new Rect(510, 10, 125, 40), "Печать")) { Save(); Print(); }

        if (GUI.Button(new Rect(515, 750, 125, 100), "Информация"))
        {
            sloy = 2;
            osz = 0;
            for (int i = 0; i < 10; i++)
            {

                ZAK = (voda[i].item.kolDostt - voda[i].item.serpro) * voda[i].item.serproZakup;//расчет денег на закупку товара
               
                osz = osz + ZAK;
            }

        }

        if (GUI.Button(new Rect(660, 750, 160, 25), "Внести в Кассу")) { cassa = cassa + cassaplus; textcassa = ""; Save(); }//конвертация текста в число
                      
        if (sloy == 3&&GUI.Button(new Rect(840, 750, 125, 100), "ОК")|| sloy == 4 && GUI.Button(new Rect(840, 750, 125, 100), "ОК") || sloy == 1 && GUI.Button(new Rect(840, 750, 125, 100), "ОК") || sloy == 2 && GUI.Button(new Rect(840, 750, 125, 100), "ОК"))
        {
            for (int i = 0; i < 10; i++)
            {
                sloy = 0;
                voda[i].item.serpro = voda[i].item.serpro + voda[i].item.serproZA;
                voda[i].item.serproZA = 0;
                voda[i].item.serproVARZkup = 0;
            }
            Save();
            Status();
        }
        
        textcassa = GUI.TextArea(new Rect(660, 800, 160, 25), textcassa, 20);//цена закупочная
        int.TryParse(textcassa, out cassaplus);
        
        
        
        #endregion

    }
    void Print()//сохраняет в файл и открывает этот файл для печати-+
    {

        StreamWriter pr =new StreamWriter("SavePrint.ods");

        pr.WriteLine("Адрес"+"     "+"Телефон"+"     "+"Кол-СП"+"   "+"Цена-СП"+"    "+"Кол-ДМ"+ "    "+"Цена-ДМ"+"    "+"Выручка");
        for (int i = 0; i < index; i++)
        {
            pr.WriteLine(massive[i].item.contrname+"   "+ massive[i].item.telephone+"     "+ massive[i].item.CECP +"    "+ massive[i].item.CPCP+ "     "+ massive[i].item.CEDM+"     "+ massive[i].item.CPDM+ "      "+ (massive[i].item.CECP *massive[i].item.CPCP + massive[i].item.CEDM * massive[i].item.CPDM));
        }
        
        pr.Close();
        System.Diagnostics.Process.Start("SavePrint.ods");
        
    }
    void Status()//функция сохраняет статус и счетает суммы
    {
        // StreamWriter status = new StreamWriter(saveSTATUS, true);//Если вторым значением стоит тру то  будет дописывать информацию снизу не затерая файл
           StreamWriter statusdat = new StreamWriter("SaveDat");
            statusdat.WriteLine(stat);
            for (int i = 0; i < stat; i++)
            {

                statusdat.WriteLine(Statis[i].item.dat);
                
            }
            statusdat.Close();

            StreamWriter status = new StreamWriter(saveSTATUS);
            status.WriteLine(rashod);
            status.WriteLine(dohod);
            status.WriteLine(kollo);
            status.WriteLine(dohlo);
            status.WriteLine(primethanie);
            status.WriteLine(col);
            status.WriteLine(koldo);
            status.WriteLine(prido);

            for (int i = 0; i < 10; i++)
            {
            
                status.WriteLine(voda[i].item.serproBAY);
                status.WriteLine(voda[i].item.serproVAR);
                status.WriteLine(voda[i].item.serproPRI);
                
            

            }

            status.WriteLine(index);
            for (int i = 0; i < index; i++)
            {

                status.WriteLine(massive[i].item.contrname);
                status.WriteLine(massive[i].item.telephone);
                status.WriteLine(massive[i].item.CECPSTR);
                status.WriteLine(massive[i].item.CEDMSTR);
                status.WriteLine(massive[i].item.CECP);
                status.WriteLine(massive[i].item.cenaCP);
                status.WriteLine(massive[i].item.CEDM);
                status.WriteLine(massive[i].item.cenaDM);
                status.WriteLine(massive[i].item.VIR);
            
            }
            if (loadTexture == Resources.Load("bleack") as Texture)
            {
                status.WriteLine("черный");
            }
            else
            {
                status.WriteLine("зеленый");
            }

            status.WriteLine(koldo);
            status.WriteLine(indeRa);
        for (int i = 0; i < indeRa; i++)
        {
              status.WriteLine(rashodOrder[i].item.dat);
              status.WriteLine(rashodOrder[i].item.Name);
              status.WriteLine(rashodOrder[i].item.VIR);
              status.WriteLine(rashodOrder[i].item.CPCP);
        }
        status.Close();

        #region сохранение данных (отказались пока что)
        /*       for (int i = 0; i < 31; i++)
           {
               Statis[stat - 1].item.dat = bufer.dat;//дата
               Statis[stat - 1].item.KolvoProd = bufer.KolvoProd + bufer.kolDostt;//кол-во проданного
               Statis[stat - 1].item.ViruthProd = bufer.ViruthProd+ bufer.viruthDostt;//выручка
               Statis[stat - 1].item.priPro = bufer.priPro+ bufer.pribDostt+dohod;//доход
               Statis[stat - 1].item.rashodMAS = rashod;//расход
               Statis[stat - 1].item.itogMAS = Statis[stat - 1].item.priPro- Statis[stat - 1].item.rashodMAS;//прибыль

               status.WriteLine(stat);
               status.WriteLine(Statis[i].item.dat);
               status.WriteLine(Statis[i].item.KolvoProd);
               status.WriteLine(Statis[i].item.ViruthProd);
               status.WriteLine(Statis[i].item.priPro);
               status.WriteLine(Statis[i].item.rashodMAS);
               status.WriteLine(Statis[i].item.itogMAS);
           }*/
        #endregion
    }
    void loadStatus()//функция загружает сохраненные продажи
    {
        StreamReader statusdat = new StreamReader(saveSTATUS);//тут мы загружаем данные продаж за день который выбрали
        int.TryParse(statusdat.ReadLine(), out rashod);
        int.TryParse(statusdat.ReadLine(), out dohod);
        int.TryParse(statusdat.ReadLine(), out kollo);
        int.TryParse(statusdat.ReadLine(), out dohlo);
        primethanie = statusdat.ReadLine();
        col = statusdat.ReadLine();
        int.TryParse(statusdat.ReadLine(), out koldo);
        int.TryParse(statusdat.ReadLine(), out prido);
        for (int y = 0; y < 10; y++)
        {

            int.TryParse(statusdat.ReadLine(), out voda[y].item.serproBAY);
            int.TryParse(statusdat.ReadLine(), out voda[y].item.serproVAR);
            int.TryParse(statusdat.ReadLine(), out voda[y].item.serproPRI);

        }
        int.TryParse(statusdat.ReadLine(), out index);
        for (int x = 0; x < index; x++)
        {
            massive[x].item.contrname = statusdat.ReadLine();
            massive[x].item.telephone = statusdat.ReadLine();
            massive[x].item.CECPSTR = statusdat.ReadLine();
            massive[x].item.CEDMSTR = statusdat.ReadLine();
            int.TryParse(statusdat.ReadLine(), out massive[x].item.CECP);
            massive[x].item.cenaCP = statusdat.ReadLine();
            int.TryParse(statusdat.ReadLine(), out massive[x].item.CEDM);
            massive[x].item.cenaDM = statusdat.ReadLine();
            int.TryParse(statusdat.ReadLine(), out massive[x].item.VIR);
        }
        if (statusdat.ReadLine() == "черный")
        {
            loadTexture = Resources.Load("bleack") as Texture;
        }
        else
        {
            loadTexture = Resources.Load("green") as Texture;
        }
        int.TryParse(statusdat.ReadLine(), out indeRa);//это нужно что бы правильно загрузить indeRa иначе он загружает koldo
        int.TryParse(statusdat.ReadLine(), out indeRa);
        for (int v = 0; v < indeRa; v++)
        {
            rashodOrder[v].item.dat = statusdat.ReadLine();
            rashodOrder[v].item.Name = statusdat.ReadLine();
            int.TryParse(statusdat.ReadLine(), out rashodOrder[v].item.VIR);
            int.TryParse(statusdat.ReadLine(), out rashodOrder[v].item.CPCP);
        }
            statusdat.Close();
    }
    void Otseel(ItemDefinee i)//функция для отсеевания контрагентов в массиве 
    {
        
        for (int x = 0; x < 200; x++){if (dublekontr[x].item == i) { o = false;}}
        if (o == true)
        {
            dublekontr[a].item.contrname = i.contrname;
            dublekontr[a].item.telephone = i.telephone;
            dublekontr[a].item.cenaCP = i.cenaCP;
            dublekontr[a].item.cenaDM = i.cenaDM;
            dublekontr[a].item.st = i.st;
        }

        if (a < 200){a++;}
       
        o = true;
       
    }
    void blabla()
    {
        s = 0;
        ns = 0;
        v = 0;
        f = 0;
        g = 0;
        h = 0;
        dney = 0;
        for (int i = b; i < c+1; i++)
        {
            
            int z;//буфер типа не куда деть

            StreamReader otbor = new StreamReader(Statis[i].item.dat);

            int.TryParse(otbor.ReadLine(), out n);//
            int.TryParse(otbor.ReadLine(), out perd);//
            int.TryParse(otbor.ReadLine(), out kollo);
            int.TryParse(otbor.ReadLine(), out dohlo);
            int.TryParse(otbor.ReadLine(), out z);
            int.TryParse(otbor.ReadLine(), out z);
            int.TryParse(otbor.ReadLine(), out koldo);
            int.TryParse(otbor.ReadLine(), out prido);
            otbor.Close();
            s = s + perd;
            ns = ns + n;
            v = v + koldo;
            f = f + prido;
            g = g + kollo;
            h = h + dohlo;
            
        }
        dney = (c - b)+1;
        dosHT = v / dney;
        dosPRI = f / dney;  
        larHT = g / dney;
        larPRI =h / dney;
        BatDos = s + f;
        BatPRI=s / dney;
    }
}
[System.Serializable]
public class intelect
{
    public ItemDefinee item;
}
   



