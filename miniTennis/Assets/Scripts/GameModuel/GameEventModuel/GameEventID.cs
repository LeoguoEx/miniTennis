using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventID
{
    public const int GAME_EVENT_START = 0;

    public const int ENTITY_START = 1;
    public const int ENTITY_HIT_BALL = 2;
    public const int ENTITY_DEAD = 3;
    public const int ENTITY_END = 100;

    public const int AI_START = 101;
    public const int AI_HIT_BALL = 102;
    public const int AI_SWITCH_STATE = 103;
    public const int AI_END = 200;

    public const int Reset_Game_State = 201;
    public const int GAME_DATA_CHANGE = 202;


    public const int New_Start = 300;
    public const int PLAYER_HIT_BALL = 301;
    public const int SWITCH_GAME_STATE = 302;
    public const int TRIGGER_GAME_EVENT = 303;
    public const int START_GAME_EVENT = 304;
    public const int END_GAME_EVENT = 305;
    public const int New_End = 400;
    
    public const int GAME_EVENT_END = 99999;
}
