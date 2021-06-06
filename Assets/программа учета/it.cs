using UnityEngine;
using System.Collections;
using System.Collections.Generic;



[System.Serializable]
public class ItemDefinee
{
    public GUIStyle st; 
    public string Name="";
    public string convert1 = "";
    public string convert2 = "";
    public string convert3 = "";
    public string dat="";
  
    public int DostavkaKOL = 0;//общ кол-во доставки                       //
    public int VIR = 0;//общая выручка доставка                            //
    public string CECPSTR = "";//цена сп стринг                            //  
    public string CEDMSTR = "";//цена сп стринг                            // блок информации доставки
    public int CECP = 0;//кол-во сп  доставка                              //
    public int CEDM = 0;//кол-во ДМ  доставка                              //
    public int kolDostt;//используется для статуса                         //
    public int viruthDostt;//используется для статуса                      //  
    public int pribDostt; //используется для статуса                       //

    public string contrname = "";//название контрагента(адрес)             //
    public string telephone = "";//№ телефона контрагента                  // блок информации контрагентов
    public string cenaCP = "";//цена серебряной прохлады для контрагента   //
    public string cenaDM = "";//цена домбая для контрагента                //
    public int CPCP = 0;//цена переведенная                                //
    public int CPDM = 0;//цена переведенная                                //

    public int serpro = 0;//остаток склада серебрянной прохлады            //
    public int serproBAY = 0;//сколько купили серебрянной прохлады         // 
    public int serproMoney = 0;//цена 90                                   // 
    public int serproVAR =0;//выручка                                      //блок информации ларька
    public int serproPRI = 0;//прибыль ларька                              //
    public int serproZakup = 0;//закупочная цена 40                        //
    public int serproZA = 0;//сколько кол-во закупили серебрянной прохлады //
    public int serproVARZkup = 0;// сколько надо отдать за закупку         //
    
}
