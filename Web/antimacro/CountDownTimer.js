<!--
var secs
var timerID = null
var timerRunning = false
var delay = 1000

function InitializeTimer(timeLeft)
{
    // Set the length of the timer, in seconds
    secs = timeLeft//+ 3600 //Uncomment this when using winter time
    StopTheClock()
    StartTheTimer()
}

function StopTheClock()
{
    if(timerRunning)
        clearTimeout(timerID)

    timerRunning = false
}

function StartTheTimer()
{
    if (secs<=0)
    {
	document.getElementById('timeLeft').innerHTML="Time left: None. Please close this window.";
        StopTheClock()
	//Close the web page
	
    }
    else
    {
        self.status = secs
        secs = secs - 1
	document.getElementById('timeLeft').innerHTML="Time left: " +secs;
        timerRunning = true
        timerID = self.setTimeout("StartTheTimer()", delay)
    }
}
//-->