
function checkForm()
{
    var loginForm = document.login;    
    
    if(checkValue(loginForm.userid,'�������û�����') ==false) return;
    if(checkValue(loginForm.pwd,'������������롣')==false) return;
    loginForm.submit();
}
function autoSend()
{
	if (event.keyCode ==13)
	{
		checkForm();
	}		
}		

function formInit()
{
    var loginFrom = document.login;
    loginFrom.userid.focus();
}