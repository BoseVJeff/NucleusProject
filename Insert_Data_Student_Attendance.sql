INSERT INTO Map_Trn_Schedule_Student_Attendance (Student,Schedule,Attendance)
SELECT 3, F16, F17
FROM OPENROWSET(
    'Microsoft.ACE.OLEDB.12.0',
    'Excel 12.0;HDR=NO;Database=C:\Users\jeffb\Desktop\Nucleus_Schedule_Data.xlsx',
    'select F16, F17 from [Sheet3$] WHERE NOT ISNULL(F17)' ) ORDER BY (SELECT 0) OFFSET 0 ROWS FETCH NEXT 482 ROWS ONLY