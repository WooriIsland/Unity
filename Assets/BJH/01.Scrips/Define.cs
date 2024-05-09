using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// enum, struct를 정의하는 클래스
// using static Define
// Define.enum.content
public class Define
{
    // request type
    public enum RequestType
    {
        GET,
        POST,
        PUT,
        DELETE,
        TEXTURE,
    }

    public enum DataType
    {
        JSON,
        CHAT,
        PHOTO,
        NET,
        NONE,
    }
}
