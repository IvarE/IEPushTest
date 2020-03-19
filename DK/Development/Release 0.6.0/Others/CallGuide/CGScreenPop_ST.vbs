rem ************************************************************************
rem Screenpop script för Skånetrafiken  
rem rev     user        datum
rem 1.0     hyr167      2014-04-29
rem 1.1     hyr167      2014-06-09
rem 1.2     hyr167      2014-06-13
rem 1.3     hyr167	2014-06-16
rem 1.4     hyr167    	2014-06-17
rem 1.5     CGI		2015-02-27
rem 1.6     CGI         2015-04-27
rem 1.7     PSV         2015-05-19 Plockat bort S i HTTP, mha Jonas Thor och Reine Rosqvist.
rem 1.8     CGI         2015-08-04 Utökat ScreenPop funktion
rem ***************************************************************************************************
rem 'UTV miljön
rem 'Const baseURL = "http://V-DKCRM-UTV:4004/CallGuide/DoAction.aspx" ' base addres for CRM posting '
rem 'Const guidURL = "http://V-DKCRM-UTV:4004/CallGuide/GetUserGuid.aspx" ' base address to retrieve Agent GUID '

rem 'Test miljön
rem 'Const baseURL = "http://V-DKCRM-TST:4004/CallGuide/DoAction.aspx" ' base addres for CRM posting '
rem 'Const guidURL = "http://V-DKCRM-TST:4004/CallGuide/GetUserGuid.aspx" ' base address to retrieve Agent GUID '

rem 'UAT miljön
Const baseURL = "http://sekunduat.skanetrafiken.se:4004/CallGuide/DoAction.aspx" ' base addres for CRM posting '
Const guidURL = "http://sekunduat.skanetrafiken.se:4004/CallGuide/GetUserGuid.aspx" ' base address to retrieve Agent GUID '

rem 'PROD miljön
rem 'Const baseURL = "http://sekund.skanetrafiken.se:4004/CallGuide/DoAction.aspx" ' base addres for CRM posting '
rem 'Const guidURL = "http://sekund.skanetrafiken.se:4004/CallGuide/GetUserGuid.aspx" ' base address to retrieve Agent GUID '


Const requestPollTimeout = 1 ' 1 second - time between each check if HTTP request has finished
Const requestTotalTimeout = 30 ' 30 seconds - maximum total time before the HTTP request times out

Const showErrorMessages = true ' if the script shall show all errors as message boxes, set to true '
Const showDebugMessages = false' if the script shall show all debug info as message boxes, set to true '

rem ************************************************************************
rem Screenpop 
rem 	
rem 
rem
rem Main ScreenPop() called by CallGuide Agent on all screenpop events
rem
rem ***************************************************************************************************
function ScreenPop()
    EventType = Params.ParamByName("popEvent")
    ContactSourceType = Params.ParamByName("contactSourceType")
    OrigContactSourceType  = Params.ParamByName("OrigContactSourceType")
    ' Både "chat" och "emailserver" har samma funktionalitet som "ivr", men vi kan hålla dem isär ändå
    ' utifall att vi behöver någon speciell hantering. Slå samman if satserna om detta inte behövs.
    if ContactSourceType = "chat" or ContactSourceType = "emailserver" then
         if EventType = "afterNormalAccept" then
                ' Ny kontakt, lagra starttiden
                contactKey = "contactStart" & Params.ParamByName("contactid")
                SessionParams.SetParamByName contactKey, CStr(Now)
         elseif EventType = "manual" then
                SendToCRM()
         end if
     elseif ContactSourceType = "ivr" then        
         if EventType = "afterNormalAccept" then
                ' Ny kontakt, lagra starttiden
                contactKey = "contactStart" & Params.ParamByName("contactid") 
                SessionParams.SetParamByName contactKey, CStr(Now) 
	 elseif EventType = "manual" then
		        SendToCRM()	    
         end if
     elseif ContactSourceType = "consultAgent" and OrigContactSourceType = "ivr" then        
         if EventType = "afterNormalAccept" then
                ' Ny kontakt, lagra starttiden
                contactKey = "contactStart" & Params.ParamByName("contactid") 
                SessionParams.SetParamByName contactKey, CStr(Now) 
	 elseif EventType = "manual" then
		        SendToCRM()	    
         end if
     else
        errorMessage = "Systemet kan inte hantera ditt val. Kontakta en administratör. Felinformation: ContactSourceType är [" & ContactSourceType & "] och OrigContactSourceType är [" & OrigContactSourceType & "]"
	x=msgbox(errorMessage ,0, "Felmeddelande")
     end if
End function

Rem ************************************************************************
Rem GetAgentGUID(), Request Agent GUID if we don't have it in the cache
Rem ************************************************************************
Function GetAgentGUID()
	if Params.ParamByName("agentname") = SessionParams.GetParamByName("lastAgentName") and SessionParams.GetParamByName("lastAgentGUID") <> "" then
		GetAgentGUID = SessionParams.GetParamByName("lastAgentGUID")
	else
		GetAgentGUID = ""
		resultCode = 0
		URL = buildGUID_URL()
		
		res = URLGet(URL, resultCode)

		if resultCode = 1 then
		  	DebugMessage "Callguide ScreenPop GetAgentGUID OK!" & vbCrLf & res, 0, "ScreenPop OK"

		  	SessionParams.SetParamByName "lastAgentName", Params.ParamByName("agentname")
		  	SessionParams.SetParamByName "lastAgentGUID", Trim(res)
			GetAgentGUID = res

		elseif resultCode = 10 then
		  	ErrorMessage "Callguide ScreenPop timeout (readyState=" & res & ") when requesting page " & vbCrLf & URL, 0, "ScreenPop Timeout"
		elseif resultCode = 11 then
			ErrorMessage "Callguide ScreenPop cannot access page " & vbCrLf & URL & vbCrLf & "request returned: " & res, 0, "ScreenPop Error"
        elseif resultCode = 12 then
            ErrorMessage "Callguide ScreenPop cannot access page " & vbCrLf & URL, 0, "ScreenPop Error"
		else
		  	ErrorMessage "Callguide ScreenPop script error, unknown return code from URLGet", 0, "ScreenPop Error"
		end if
	end if
End Function 

Rem ************************************************************************
Rem SendToCRM(), Pass data to CRM system
Rem ************************************************************************
Function SendToCRM()
	agentGUID = GetAgentGUID()

	resultCode = 0
	URL = buildURL(agentGUID)
	DebugMessage "Callguide ScreenPop GetAgentGUID OK!" & vbCrLf & URL , 0, "ScreenPop OK"
	res = URLGet(URL, resultCode)

	if resultCode = 1 then
	  	DebugMessage "Callguide ScreenPop OK!" & vbCrLf & res, 0, "ScreenPop OK"

	    OpenIE res
	elseif resultCode = 10 then
	  	ErrorMessage "Callguide ScreenPop timeout (readyState=" & res & ") when requesting page " & vbCrLf & URL, 0, "ScreenPop Timeout"
	elseif resultCode = 11 then
		ErrorMessage "Callguide ScreenPop cannot access page " & vbCrLf & URL & vbCrLf & "request returned: " & res, 0, "ScreenPop Error"
    elseif resultCode = 12 then
        ErrorMessage "Callguide ScreenPop cannot access page " & vbCrLf & URL, 0, "ScreenPop Error"
	else
	  	ErrorMessage "Callguide ScreenPop script error, unknown return code from URLGet", 0, "ScreenPop Error"
	end if
	SendToCRM = resultCode
End Function

Rem ************************************************************************
Rem Sleep_v1(ms), sleep For ms milliseconds. This is an asynchrony sleep!
Rem ************************************************************************
Sub Sleep_v1(ms)
  WScript.Sleep(ms)
End Sub

Rem ************************************************************************
Rem Sleep_v2(ms), sleep For ms milliseconds. This is an asynchrony sleep!
Rem ************************************************************************
Sub Sleep_v2(ms)
	Set oAutoIt = CreateObject("AutoItX.Control")
	oAutoIt.Sleep(ms)
	Set oAutoIt = Nothing
End Sub

Rem ************************************************************************
Rem Sleep_v3(ms), sleep For ms milliseconds. This is an asynchrony sleep!
Rem ************************************************************************
Sub Sleep_v3(ms)
  Set UtilComp = CreateObject("UtilComp.DDE")
  UtilComp.Sleep(ms)
  Set UtilComp = Nothing
End Sub

Rem ************************************************************************
Rem buildURL, builds the full URL to post
Rem ************************************************************************
Function buildURL(agentGUID)

	ct = ""
	if Params.ParamByName("popEvent") = "endOfContact" then 
		ct = calculateContactTime()
	end if

    ' The parameters may contain characters forbidden in URLs, so we encode them'
    result = baseURL
    result = result & "?contactid=" & URLEncode(Params.ParamByName("contactid"))
    result = result & "&queuetime=" & URLEncode(Params.ParamByName("queuetime"))
    result = result & "&aphonenumber=" & URLEncode(Params.ParamByName("Ani"))
    result = result & "&bphonenumber=" & URLEncode(Params.ParamByName("Dnis"))
    result = result & "&errand=" & URLEncode(Params.ParamByName("errand"))
    result = result & "&screenpopchoice=" & URLEncode(Params.ParamByName("popEvent"))
    result = result & "&contactsourcetype=" & URLEncode(Params.ParamByName("contactSourceType"))
    result = result & "&cid=" & URLEncode(Params.ParamByName("cid")) 
    result = result & "&chatcustomeralias=" & ReplaceSpace(URLEncode(Params.ParamByName("chatCustomerAlias")))
    result = result & "&agentname=" & ReplaceSpace(URLEncode(Params.ParamByName("agentName")))
    result = result & "&contacttime=" & URLEncode(ct)
    result = result & "&agentid=" & URLEncode(agentGUID)

    buildURL = result
End Function

Rem ************************************************************************
Rem buildGUID_URL, builds the full GUID retrieval URL 
Rem ************************************************************************
Function buildGUID_URL()

    ' The parameters may contain characters forbidden in URLs, so we encode them'
    result = guidURL
    result = result & "?agentname=" & ReplaceSpace(URLEncode(Params.ParamByName("agentName")))

    buildGUID_URL = result
End Function

Rem ************************************************************************
Rem calculateContactTime, calculate how many seconds we have been in call/chat/fb
Rem and clear the stored session data
Rem ************************************************************************
Function calculateContactTime()
    contactKey = "contactStart" & Params.ParamByName("contactid")
    contactStart = SessionParams.GetParamByName(contactKey)
    contactEnd = CStr(Now)                
    
    ' Clear the session data
    SessionParams.SetParamByName contactKey, ""

    if not IsDate(contactStart) or not IsDate(contactEnd) then
    	' Invalid dates
    	calculateContactTime = ""    	
    	exit function
    End If

    ' Present call time in minutes with two decimal places '
    calculateContactTime = Replace(Round(DateDiff("s", contactStart, contactEnd) / 60.0, 2), ",", ".")
End Function

Rem ************************************************************************
Rem URLGet(URL), silently makes a HTTP request in the background
Rem If any text is returned from the HTTP request, an IE window will be opened
Rem towards that text (treated as an URL)
Rem
Rem resultCode is an output parameter. Coding:
Rem  1  - the HTTP request was successful, function returns a string with the response
Rem  10 - Timeout when requesting page, function returns readyState of HTTP request
Rem  11 - Access error (HTTP returned other pagestatus than 200), function returns HTTP pagestatus
Rem  12 - Early access error (HTTP helper object raised and error, could be TCP timeout), function returns empty string
Rem ************************************************************************
Function URLGet(URL, resultCode)
  On Error Resume Next
  Set Http = CreateObject("Msxml2.ServerXMLHTTP")
  
  ' För synkront anrop by till'
  'Http.Open "GET", URL, False
  Http.Open "GET", URL, True
  Http.Send

  requestStartTime = Now
  currentTime = requestStartTime
  Do While (Err.number = 0 and Http.readyState <> 4) And (DateDiff("s", requestStartTime, currentTime) < requestTotalTimeout)
    Http.waitForResponse(requestPollTimeout)
  	currentTime = Now
  Loop 

  if Err.number <> 0 then
    ' We got and error from the HTTP object
    resultCode = 12
    URLGet = ""
    Http.Abort
    exit Function
  end if
  On Error Goto 0

  if Http.readyState <> 4 then
  	' We could not complete the request in time
  	resultCode = 10
    URLGet = Http.readyState
    Http.Abort
    exit Function
  end if

  pagestatus = Http.status
  if pagestatus<>"200" then
  	' Response not OK '
  	resultCode = 11
    URLGet = pagestatus
  else
  	resultCode = 1
    URLGet = Trim(Http.responseText)
  end if
End Function


Rem ************************************************************************
Rem OpenIE(URL), Opens an IE window in the foreground if URL is non-empty
Rem ************************************************************************
Sub OpenIE(URL)
	if Not VarType(URL) = vbString then
		exit sub
	end if

	if Len(Trim(URL)) = 0 then
		exit sub	
	end if	

	Set objIE = CreateObject("InternetExplorer.Application")
	objIE.Visible = 1
	objIE.Navigate Trim(URL)
End Sub

Rem ************************************************************************
Rem URLEncode(Text)
Rem convert a string so that it can be used on a URL query string
Rem Same effect as the Server.URLEncode method in ASP
Rem ************************************************************************
Function URLEncode(Text)
    URLEncodeText = Text
    
    For i = Len(URLEncodeText) To 1 Step -1
        acode = Asc(Mid(URLEncodeText, i, 1))
	
		' 0-9'							'A-Z'							'a-z'							'~'					'_'				'-'				'.'
        If ((acode < 48 or acode > 57) and (acode < 65 or acode > 90) and (acode < 97 or acode > 122) and (acode <> 126) and (acode <> 95) and (acode <> 45) and (acode <> 46)) Then
	 	if acode = 32 then
	        	' replace space with "+"
			'Do nothing. This is handled in function ReplaceSpace.
	        else
	            	' replace punctuation chars with "%hex"
	            	URLEncodeText = Left(URLEncodeText, i - 1) & "%" & Hex(acode) & Mid(URLEncodeText, i + 1)
	        End If        
        End if 
    Next
    
    URLEncode = URLEncodeText

End Function

Rem ************************************************************************
Rem ReplaceSpace(Text)
Rem convert a string so that it can be used on a URL query string
Rem replace space with %20
Rem ************************************************************************
Function ReplaceSpace(text)
	replacetext = Replace(text," ","%20",1,-1)
	ReplaceSpace = replacetext
End Function


Rem ************************************************************************
Rem ErrorMessage, Conditional logging of error messages
Rem ************************************************************************
Sub ErrorMessage(msg, msgtype, title)
	if showErrorMessages then
		MsgBox msg, msgtype, title
	end if
End Sub

Rem ************************************************************************
Rem DebugMessage, Conditional logging of debug messages
Rem ************************************************************************
Sub DebugMessage(msg, msgtype, title)
	if showDebugMessages then
		MsgBox msg, msgtype, title
	end if
End Sub

