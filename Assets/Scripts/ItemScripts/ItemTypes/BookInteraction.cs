using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookInteraction : MonoBehaviour
{
    /*Script Values

        0 - Test Book


        */

    public Item item;

    public void ProcessBook()
    {
        ReadBook(item.scriptValue);
    }

    void ReadBook(int bookIndex)
    {
        switch (bookIndex)
        {
            case 0: //Test book
                
                break;
        }
    }
}
