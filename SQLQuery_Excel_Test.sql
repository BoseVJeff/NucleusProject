SELECT *
FROM OPENROWSET(
    'Microsoft.ACE.OLEDB.12.0',
    'Excel 12.0;HDR=NO;Database=C:\Users\jeffb\Desktop\Nucleus_Schedule_Data.xlsx',
    'select * from [Sheet2$]')