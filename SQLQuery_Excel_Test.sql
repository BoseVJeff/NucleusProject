SELECT F1, F2, F4, F5, F6
FROM OPENROWSET(
    'Microsoft.ACE.OLEDB.12.0',
    'Excel 12.0;HDR=NO;Database=C:\Users\jeffb\Desktop\Nucleus_Schedule_Data.xlsx',
    'select F1, F2, F4, F5, F6 from [Sheet3$]') ORDER BY (SELECT 0) OFFSET 0 ROWS FETCH NEXT 482 ROWS ONLY