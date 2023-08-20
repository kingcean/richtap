#pragma once
#include<string>


#define MYDLLAPI  __declspec(dllexport)

extern "C" {

typedef void(*RICHTAP_CALLBACK)(char * data, int size);

MYDLLAPI  void RichTapInit();

MYDLLAPI  void RichTapSetCallback(RICHTAP_CALLBACK cb);

MYDLLAPI  void RichTapDestroy();

MYDLLAPI  void RichTapPlay(const char* strHE, const int loop = 0, const int interval = 0, const int intensityFactor = 255, const int freqFactor = 0);

MYDLLAPI  void RichTapPlaySection(const char* strHE, const int loop = 0, const int interval = 0, const int intensityFactor = 255, const int freqFactor = 0 , const int start = 0, const int end = INT_MAX);

MYDLLAPI  void RichTapStop();

MYDLLAPI  void RichTapSendLoopParam(const int interval, const int intensityFactor, const int freqFactor);

MYDLLAPI  void RichTapSetTrigger(const int index, const int mode, const int amplitude, const int frequency, const int resistive, const int startPosition, const int endPosition);

MYDLLAPI const char * RichTapGetConnectedGameControllers();

MYDLLAPI const char * RichTapGetVersion();

MYDLLAPI void EnableDebugLog(bool enable);

}

