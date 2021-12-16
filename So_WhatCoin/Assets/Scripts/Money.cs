using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class Money : MonoBehaviour
{
    static string[] unitSymbol = new string[] { "", "��", "��", "��", "��", "��" };

    // long ���� double�� �ִ� ���� Ŀ�� double  ���
    public static string ToString(ulong value)
    {
        if (value == 0) { return "0"; }

        int unitID = 0;

        string number = string.Format("{0:# #### #### #### #### ####}", value).TrimStart();
        string[] splits = number.Split(' ');

        StringBuilder sb = new StringBuilder();

        for (int i = splits.Length; i > 0; i--)
        {
            int digits = 0;
            if (int.TryParse(splits[i - 1], out digits))
            {
                // ���ڸ��� 0�� �ƴҶ�
                if (digits != 0)
                {
                    sb.Insert(0, $"{ digits}{ unitSymbol[unitID] }");
                }
            }
            else
            {
                // ���̳ʽ��� ���ڿ� ����
                sb.Insert(0, $"{ splits[i - 1] }");
            }
            unitID++;
        }
        return sb.ToString();
    }
}
