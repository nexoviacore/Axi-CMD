var startTag = "<div style=\"background-color:#FFFFFF;font-size:13px;color:#2B4E22;width:200px; height:auto; padding:2px; margin:3px; border:1px solid #B5C3B1\">";
  
  
 var endTag ="</div>";

//Window Title End
// position of the tooltip relative to the mouse in pixel //
var offsetx = 12;
var offsety =  8;

function newelement(newid)
{ 
    if(document.createElement)
    { 
        var el = document.createElement('div'); 
        el.id = newid;     
        with(el.style)
        { 
            display = 'none';
            position = 'absolute';
        } 
        el.innerHTML = '&nbsp;'; 
        document.body.appendChild(el); 
    } 
} 
var ie5 = (document.getElementById && document.all); 
var ns6 = (document.getElementById && !document.all); 
var ua = navigator.userAgent.toLowerCase();
var isapple = (ua.indexOf('applewebkit') != -1 ? 1 : 0);

function mmtooltip(tip,id)
{
  
	if( tip == null || tip == "" ) return ;
	if(document.readyState !='complete' ) return;
    if(!document.getElementById('mmtooltip')) newelement('mmtooltip');
    var m4m_tooltip = document.getElementById('mmtooltip');
    
	if( m4m_tooltip ){
	   	m4m_tooltip.innerHTML = startTag+ tip +endTag ; 
		m4m_tooltip.style.display = 'block'; 
		
		var position = Sys.UI.DomElement.getBounds(document.getElementById('form1'));
		x=position.x+840;
		position = Sys.UI.DomElement.getBounds(document.getElementById(id));
		y= position.y - 5;		 
		Sys.UI.DomElement.setLocation(m4m_tooltip, x, y);
	
  }
}

function exit()
{
	try {
    document.getElementById('mmtooltip').style.display = 'none';
	}catch(error){}
}

document.ondragstart = function () { return false; };
