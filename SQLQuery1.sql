INSERT INTO Restaurant(Rname,Category,Rimages)
SELECT 'Taj', 'VEG',Rimages
FROM OPENROWSET(BULK N'E:\Images\Taj_mumbai.jpg', SINGLE_BLOB) AS ImageSource(Rimages);

INSERT INTO Restaurant(Rname,Category,Rimages)
SELECT 'Oberoi', 'NON-VEG',Rimages
FROM OPENROWSET(BULK N'E:\Images\oberoi_delhi.jpg', SINGLE_BLOB) AS ImageSource(Rimages);

INSERT INTO Branch(Rname, bcity, Location, bphonenum, bimage)
SELECT 'Taj','Mumbai','second street',241632,bimage
FROM OPENROWSET(BULK 'E:\Images\Taj_mumbai.jpg', SINGLE_BLOB) AS ImageSource(bimage);

INSERT INTO Branch(Rname, bcity, Location, bphonenum, bimage)
SELECT 'Oberoi','Delhi','Rohini',2415632,bimage
FROM OPENROWSET(BULK 'E:\Images\oberoi_delhi.jpg', SINGLE_BLOB) AS ImageSource(bimage);
