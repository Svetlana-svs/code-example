#pragma once

#include "XMLSettingsReader.h"

#define VIDEOCHANNUM	1	// Number of video channels. Must be 1 .. 16

class Settings
{
public:
	Settings(void);
	~Settings(void);
	bool Load();
	MapStringString GeViScope, Database, Channels[VIDEOCHANNUM];
	struct
	{
		MapStringString Detection;
		MapStringString Recognition;
	} Verilook;
};
