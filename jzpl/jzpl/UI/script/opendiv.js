
var W = screen.width;//ȡ����Ļ�ֱ��ʿ��
        var H = screen.height;//ȡ����Ļ�ֱ��ʸ߶�
        var x;
        var y;
        function mouseMove(ev)
        {
         ev= ev || window.event;
          var mousePos = mouseCoords(ev);
          //alert(ev.pageX);
              x = mousePos.x;
              y = mousePos.y;
        }

        function mouseCoords(ev)
        {
         if(ev.pageX || ev.pageY){
           return {x:ev.pageX, y:ev.pageY};
         }
         return {
             x:ev.clientX + document.body.scrollLeft - document.body.clientLeft,
             y:ev.clientY + document.body.scrollTop  - document.body.clientTop
         };
        }

        document.onmousemove = mouseMove;

        function M(id)
        {
            return document.getElementById(id);//��M()��������document.getElementById(id)
        }
        function MC(t)
        {
           return document.createElement(t);//��MC()��������document.createElement(t)
        };
        //�ж�������Ƿ�ΪIE
        function isIE()
        {
              return (document.all && window.ActiveXObject && !window.opera) ? true : false;
        }
        //ȡ��ҳ��ĸ߿�
        function getBodySize()
        {
           var bodySize = [];
           with(document.documentElement) {
            bodySize[0] = (scrollWidth>clientWidth)?scrollWidth:clientWidth;//����������Ŀ�ȴ���ҳ��Ŀ�ȣ�ȡ�ù������Ŀ�ȣ�����ȡҳ����
            bodySize[1] = (scrollHeight>clientHeight)?scrollHeight:clientHeight;//����������ĸ߶ȴ���ҳ��ĸ߶ȣ�ȡ�ù������ĸ߶ȣ�����ȡ�߶�
           }
           return bodySize;
        }
        //�����ڸǲ�
        function popCoverDiv()
        {
           if (M("cover_div")) 
           {
           //��������ڸǲ㣬��������ʾ
            M("cover_div").style.display = 'block';
           } 
           else 
           {
           //���򴴽��ڸǲ�
            var coverDiv = MC('div');
            document.body.appendChild(coverDiv);
            coverDiv.id = 'cover_div';
            with(coverDiv.style)
            {
                 position = 'absolute';
                 background = '#B2B2B2';
                 left = '0px';
                 top = '0px';
                 var bodySize = getBodySize();
                 width = bodySize[0] + 'px'
                 height = bodySize[1] + 'px';
                 zIndex = 0;
                 if (isIE())
                 {
                  filter = "Alpha(Opacity=60)";//IE�澳
                 } else {
                  opacity = 0.6;
                 }
            }
           }
        }

        function showLogin(eid)
        {
            var login=M(eid);
            login.style.display = "block";
        }

        //����DIV�����ʽ
        function change(eid)
        {
              var login = M(eid);
              login.style.position = "absolute";
              login.style.border = "1px solid #CCCCCC";
              login.style.background ="#F6F6F6";
              var i=0;
                  var bodySize = getBodySize();
//              login.style.left = (bodySize[0]-i*i*4)/2+"px";
//              login.style.top = (bodySize[1]/2-100-i*i)+"px";
              login.style.left =30+"px"; //x;
              login.style.top = 30+"px";//y;
              login.style.width =500 + "px";
              login.style.height = 200+ "px";
              
              //popChange(i,eid);
        }
        //��DIV���Сѭ������
        function popChange(i,devid)
        {
              var login = M(devid);
                  var bodySize = getBodySize();
              login.style.left = x;//(bodySize[0]-i*i*4)/2+"px";
              login.style.top = y;//(bodySize[1]/2-100-i*i)+"px";
              login.style.width =50+ "px";
              login.style.height = i*i*1+ "px";
              
              if(i<=10){
                   i++;
                   setTimeout("popChange("+i+","+devid+")",10);//���ó�ʱ10����
              }
        }
        //��DIV��
        function opendiv(divid)
        {
                change(divid);
                showLogin(divid);
                //popCoverDiv()
                //void(0);//�������κβ���,�磺<a href="#">aaa</a>
        }
        //�ر�DIV��
        function closediv(divid)
        {
                 M(divid).style.display = 'none';
                 //M("cover_div").style.display = 'none';
                void(0);
        }
        function ShowFJ(obj)
        {
            //alert(obj);
            //var DivID = document.getElementById(grd_fj[Number(obj)]).id;
            opendiv(obj);
        }
        function CloseFJ(obj)
        {
            var DivID = document.getElementById(grd_fj[Number(obj)]).id;
            closediv(DivID);
        }