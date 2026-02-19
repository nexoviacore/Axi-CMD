
-- fn_axi_struct_metadata

CREATE OR REPLACE FUNCTION fn_axi_struct_metadata(pstructtype character varying, ptransid character varying, pobjtype character varying)
 RETURNS TABLE(objtype character varying, objcaption text, objname character varying, dcname character varying, asgrid character varying)
 LANGUAGE plpgsql
AS $function$
declare 
declare v_sql varchar;
begin
 if pstructtype='tstruct' then
  if pobjtype = 'fields' then 
  v_sql = 'select ''Field''::varchar,caption||''(''||fname||'')'',fname,dcname,asgrid from axpflds where tstruct =$1';
  elsif pobjtype = 'genmaps' then 
  v_sql = 'select ''Genmap''::varchar,caption||''(''||gname||'')'',gname,null::varchar,null::varchar from axpgenmaps where tstruct =$1';
  elsif pobjtype = 'mdmaps' then 
  v_sql = 'select ''MDmap''::varchar,caption||''(''||mname||'')'',mname,null::varchar,null::varchar from axpmdmaps where tstruct =$1';  
  end if;
 elsif pstructtype='iview' then
  if pobjtype = 'columns' then
  v_sql = 'select ''Column''::varchar,f_caption||''(''||f_name||'')'',f_name,null::varchar,null::varchar from iviewcols where iname =$1';  
  elsif pobjtype = 'params' then
  v_sql = 'select ''Param''::varchar,pcaption||''(''||pname||'')'',pname,null::varchar,null::varchar from iviewparams where iname =$1';  
  end if; 
end if;

return query execute v_sql using ptransid;

END; 
$function$
;
 
 
------------------------------------

SELECT * from fn_axi_struct_metadata('tstruct', 'b_sql', 'fields');
 
 
SELECT * from fn_axi_struct_metadata('tstruct', 'axusr', 'genmaps');
 
 
SELECT * from fn_axi_struct_metadata('tstruct', 'ad__g', 'mdmaps');
 
 
SELECT * from fn_axi_struct_metadata('iview', 'pservers', 'columns');
 
 
SELECT * from fn_axi_struct_metadata('iview', 'ad___upm', 'params');