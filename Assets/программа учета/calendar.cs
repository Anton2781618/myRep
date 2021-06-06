using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
public class calendar : MonoBehaviour
{
    [System.Serializable]
    public class day//класс хер его знает для чего 
    {
        public int dayNum;//номер дня
        public Color dayColor;//цвет ячейки дня
        public GameObject obj;//кнопка дня

        public day(int dayNum, Color dayColor, GameObject obj)
        {
           
            this.dayNum = dayNum;
            this.dayColor = dayColor;
            this.obj = obj;
            obj.GetComponent<Image>().color = dayColor;
            UpdateDay(dayNum);
        }

        public void UpdateColor(Color newColor)
        {
            obj.GetComponent<Image>().color = newColor;
            dayColor = newColor;
        }

        public void UpdateDay(int newDayNum)
        {
            this.dayNum = newDayNum;
            if (dayColor == Color.white || dayColor == Color.green)
            {
                obj.GetComponentInChildren<Text>().text = (dayNum + 1).ToString();
            }
            else
            {
                obj.GetComponentInChildren<Text>().text = "";
            }
        }
    }
    
    public List<day> days = new List<day>();
    public Transform[] weeks;
    public Text MouthAndYear;
    public DateTime currDate = DateTime.Now;

    private void Start()
    {
        UpdateCalendar(DateTime.Now.Year, DateTime.Now.Month);
    }

    void UpdateCalendar(int year, int mounth)
    {
        DateTime temp = new DateTime(year, mounth, 1);
        currDate = temp;
        MouthAndYear.text = temp.ToString("MM") + "." + temp.Year.ToString();
        int startDay = GetMounthStartDay(year, mounth);
        int endDay = GetTotalNumberOfDay(year, mounth);

        if (days.Count == 0)
        {
            for (int w = 0; w < 6; w++)
            {
                for (int i = 0; i < 7; i++)//наверное 7 дней
                {
                    day newDay;
                    int currDay = (w * 7) + i;

                    if (currDay < startDay || currDay - startDay >= endDay)
                    {
                        newDay = new day(currDay - startDay, Color.gray, weeks[w].GetChild(i).gameObject);//заполняем серые дни
                    }
                    else
                    {
                        newDay = new day(currDay - startDay, Color.white, weeks[w].GetChild(i).gameObject);//заполняем белые дни
                    }
                    days.Add(newDay);//добавляем в массив который сериализуем дни
                }
            }
        }
        else
        {
            for (int i = 0; i < 42; i++)
            {
                if (i < startDay || i - startDay >= endDay)
                {
                    days[i].UpdateColor(Color.gray);
                }
                else
                {
                    days[i].UpdateColor(Color.white);
                }
                days[i].UpdateDay(i - startDay);
            }

        }

        if (DateTime.Now.Year == year && DateTime.Now.Month == mounth)
        {
            days[(DateTime.Now.Day - 1) + startDay].UpdateColor(Color.green);//двигает зеленый указатель
        }
    }

    int GetMounthStartDay(int year, int mounth)
    {
        DateTime temp = new DateTime(year, mounth, 1);
        return (int)temp.DayOfWeek;
    }

    int GetTotalNumberOfDay(int year, int mounth)
    {
        return DateTime.DaysInMonth(year, mounth);
    }

    public void SwithMounth(int Direction)
    {
        if (Direction < 0)
        {
            currDate = currDate.AddMonths(-1);
        }
        else
        {
            currDate = currDate.AddMonths(1);
        }
        UpdateCalendar(currDate.Year, currDate.Month);
    }

    public void SwithColor(Image ColorObject)//в скобках сам обьект у которого надо поменять цвет
    {    
        if(ColorObject.GetComponent<Image>().color!=Color.grey)
        {    
            for (int i = 0; i < days.Count; i++)
            {
                if(days[i].dayColor!=Color.gray && days[i].dayColor!=Color.green)
                {
                    days[i].UpdateColor(Color.white);
                }
            }
            
                days[(DateTime.Now.Day - 1) + GetMounthStartDay(DateTime.Now.Year, DateTime.Now.Month)].UpdateColor(Color.green);
            
            ColorObject.GetComponent<Image>().color=Color.blue; //74BC7B
        }
    }
}
