SELECT *
FROM OPENROWSET(
    'Microsoft.ACE.OLEDB.12.0',
    'Excel 12.0;HDR=NO;Database=C:\Users\jeffb\Desktop\Entr_Anal.xlsx',
    'select F1,F2,F12 from [Token Pricing$]')