
--fn_axi_metadata

CREATE OR REPLACE FUNCTION fn_axi_metadata(pstructtype character varying, pusername character varying)
 RETURNS TABLE(structtype text, caption text, transid character varying)
 LANGUAGE plpgsql
AS $function$
declare 
declare v_sql varchar;
begin
 if pstructtype='tstructs' then
  v_sql = 'select ''tstruct'',caption || ''-('' || name || '')'' cap,name
 from axusergroups a3 join axusergroupsdetail a4 on a3.axusergroupsid = a4.axusergroupsid
 join axuseraccess a5 on a4.roles_id = a5.rname
 join tstructs t on a5.sname = t.name
 join axuserlevelgroups ag on a3.groupname = ag.usergroup 
 where t.blobno =1 and ag.username =$1
 union all
 select ''tstruct'',caption || ''-('' || name || '')'' cap,name
 from tstructs t 
 join axuserlevelgroups ag on ag.usergroup =''default''
 where t.blobno =1 and ag.username =$1
 union all
       SELECT ''tstruct'',caption || ''-('' || name || '')'' cap,name from tstructs t 
       JOIN axuserlevelgroups u on u.USERNAME = $1
     join axusergroups a ON a.groupname = u.usergroup 
        JOIN axusergroupsdetail b ON a.axusergroupsid = b.axusergroupsid        
        where b.ROLES_ID =''default'' and t.blobno =1';        
 elsif pstructtype='iviews' then
  v_sql = 'select ''iview'',caption || ''-('' || name || '')'' cap,name
 from axusergroups a3 join axusergroupsdetail a4 on a3.axusergroupsid = a4.axusergroupsid
 join axuseraccess a5 on a4.roles_id = a5.rname
 join iviews t on a5.sname = t.name
 join axuserlevelgroups ag on a3.groupname = ag.usergroup 
 where t.blobno =1 and ag.username =$1
 union all
 select ''iview'',caption || ''-('' || name || '')'' cap,name
 from iviews t 
 join axuserlevelgroups ag on ag.usergroup =''default''
 where t.blobno =1 and ag.username =$1
 union all
       SELECT ''iview'',caption || ''-('' || name || '')'' cap,name from iviews t 
       JOIN axuserlevelgroups u on u.USERNAME = $1
     join axusergroups a ON a.groupname = u.usergroup 
        JOIN axusergroupsdetail b ON a.axusergroupsid = b.axusergroupsid        
        where b.ROLES_ID =''default'' and t.blobno =1';
elsif pstructtype='pages' then 
 v_sql = 'select ''page'',caption || ''-('' || name || '')'' cap,name
 from axusergroups a3 join axusergroupsdetail a4 on a3.axusergroupsid = a4.axusergroupsid
 join axuseraccess a5 on a4.roles_id = a5.rname
 join axpages p on a5.sname = p.name and p.pagetype =''web''
 join axuserlevelgroups ag on a3.groupname = ag.usergroup 
 where ag.username =$1
 union all
 select ''page'',caption || ''-('' || name || '')'' cap,name
 from axpages t 
 join axuserlevelgroups ag on ag.usergroup =''default''
 where t.pagetype =''web'' and ag.username =$1
 union all
       SELECT ''page'',caption || ''-('' || name || '')'' cap,name from axpages t 
       JOIN axuserlevelgroups u on u.USERNAME = $1
     join axusergroups a ON a.groupname = u.usergroup 
        JOIN axusergroupsdetail b ON a.axusergroupsid = b.axusergroupsid        
        where b.ROLES_ID =''default'' and t.pagetype =''web''';   
elsif pstructtype='ads' then  
v_sql = 'select ''ADS'',sqlname::text,sqlname from axdirectsql';
else
v_sql = 'select ''tstruct'',caption || ''-('' || name || '')'' cap,name
 from axusergroups a3 join axusergroupsdetail a4 on a3.axusergroupsid = a4.axusergroupsid
 join axuseraccess a5 on a4.roles_id = a5.rname
 join tstructs t on a5.sname = t.name
 join axuserlevelgroups ag on a3.groupname = ag.usergroup 
 where t.blobno =1 and ag.username =$1
 union all
 select ''tstruct'',caption || ''-('' || name || '')'' cap,name
 from tstructs t 
 join axuserlevelgroups ag on ag.usergroup =''default''
 where t.blobno =1 and ag.username =$1
 union all
       SELECT ''tstruct'',caption || ''-('' || name || '')'' cap,name from tstructs t 
       JOIN axuserlevelgroups u on u.USERNAME = $1
     join axusergroups a ON a.groupname = u.usergroup 
        JOIN axusergroupsdetail b ON a.axusergroupsid = b.axusergroupsid        
        where b.ROLES_ID =''default'' and t.blobno =1
union all  
select ''iview'',caption || ''-('' || name || '')'' cap,name
 from axusergroups a3 join axusergroupsdetail a4 on a3.axusergroupsid = a4.axusergroupsid
 join axuseraccess a5 on a4.roles_id = a5.rname
 join iviews t on a5.sname = t.name
 join axuserlevelgroups ag on a3.groupname = ag.usergroup 
 where t.blobno =1 and ag.username =$1
 union all
 select ''iview'',caption || ''-('' || name || '')'' cap,name
 from iviews t 
 join axuserlevelgroups ag on ag.usergroup =''default''
 where t.blobno =1 and ag.username =$1
 union all
       SELECT ''iview'',caption || ''-('' || name || '')'' cap,name from iviews t 
       JOIN axuserlevelgroups u on u.USERNAME = $1
     join axusergroups a ON a.groupname = u.usergroup 
        JOIN axusergroupsdetail b ON a.axusergroupsid = b.axusergroupsid        
        where b.ROLES_ID =''default'' and t.blobno =1
union all
select ''page'',caption || ''-('' || name || '')'' cap,name
 from axusergroups a3 join axusergroupsdetail a4 on a3.axusergroupsid = a4.axusergroupsid
 join axuseraccess a5 on a4.roles_id = a5.rname
 join axpages p on a5.sname = p.name and p.pagetype =''web''
 join axuserlevelgroups ag on a3.groupname = ag.usergroup 
 where ag.username =$1
 union all
 select ''page'',caption || ''-('' || name || '')'' cap,name
 from axpages t 
 join axuserlevelgroups ag on ag.usergroup =''default''
 where t.pagetype =''web'' and ag.username =$1
 union all
       SELECT ''page'',caption || ''-('' || name || '')'' cap,name from axpages t 
       JOIN axuserlevelgroups u on u.USERNAME = $1
     join axusergroups a ON a.groupname = u.usergroup 
        JOIN axusergroupsdetail b ON a.axusergroupsid = b.axusergroupsid        
        where b.ROLES_ID =''default'' and t.pagetype =''web''
union all
select ''ADS'',sqlname::text,sqlname from axdirectsql';
end if;

return query execute v_sql using pusername;

END; 
$function$
;


-----------------------------

SELECT * from fn_axi_metadata('tstructs', 'admin');
 
 
SELECT * from fn_axi_metadata('iviews', 'admin');
 
 
SELECT * from fn_axi_metadata('pages', 'admin');
 
 
SELECT * from fn_axi_metadata('ads', 'admin');
 
SELECT * from fn_axi_metadata('all', 'admin');