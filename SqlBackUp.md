
## axi_adscolumnlist
```sql 
select b.fldcaption || '('||b.fldname||')' displaydata,b.fldname name,b.fldcaption caption,b.normalized,b.fdatatype, b.sourcetable,b.sourcefld , CASE WHEN lower(sqltext) LIKE '%--axp_filter%' THEN 'T' ELSE 'F' END AS filters from axdirectsql a left join axdirectsql_metadata b on a.axdirectsqlid =b.axdirectsqlid where sqlname = :param1

```