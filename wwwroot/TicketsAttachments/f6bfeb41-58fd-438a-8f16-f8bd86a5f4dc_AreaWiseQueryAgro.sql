Select SaleManID,SalesMan,SectorName,SectorID,sum(OpeningBalance) as OpeningBalance,sum(TotalSale) as TotalSale,SUm(TotalReturn) as SalesReturn,sum(DebitJV) as DebitJv,sum(PrRecovery) as PrRecovery,sum(Purchase)  as TotalPurchase,sum(CreditJv) as CreditJv from (
Select ISNULL(SalesMan.SaleManInfoID,0) SaleManID,Isnull(SaleManName,'No Sales Person') as SalesMan,ISNULL(SectorName,'No Sector') SectorName,ISNULL(gen_PartiesInfo.SectorID,0) as SectorID,PartyGlid, dbo.OpeningBalance('2022-03-31', gen_PartiesInfo.Companyid,PartyGLID,0,'false') AS OpeningBalance,Glvdetail.dr as TotalSale,0 as TotalReturn,0 as DebitJV,0 as PrRecovery,0 as Purchase,0 as CreditJv from gen_PartiesInfo
left join  gen_SectorInfo on gen_SectorInfo.SectorID=gen_PartiesInfo.SectorID left join gen_SaleManInfo SalesMan on SalesMan.SaleManInfoID=gen_PartiesInfo.SaleManInfoID 
left join data_SaleInfo on data_SaleInfo.PartyID=gen_PartiesInfo.PartyID
left join GLvMAIN on GLvMAIN.vID=data_SaleInfo.AccountVoucherID left join GLvDetail on GLvMAIN.vID=GLvDetail.vID and GLvDetail.GlAccountID=gen_PartiesInfo.PartyGLID

union all

Select ISNULL(SalesMan.SaleManInfoID,0) SaleManID,Isnull(SaleManName,'No Sales Person') as SalesMan,ISNULL(SectorName,'No Sector') SectorName,ISNULL(gen_PartiesInfo.SectorID,0) as SectorID,PartyGlid, 0 AS OpeningBalance,0 as TotalSale,SrVDetail.Cr as TotalReturn,0 as DebitJV,0 as PrRecovery,0 as Purchase,0 as CreditJv from gen_PartiesInfo
left join  gen_SectorInfo on gen_SectorInfo.SectorID=gen_PartiesInfo.SectorID left join gen_SaleManInfo SalesMan on SalesMan.SaleManInfoID=gen_PartiesInfo.SaleManInfoID 

left join data_SaleReturnInfo on data_SaleReturnInfo.SaleID=gen_PartiesInfo.PartyID
left join GLvMAIN SRVoucher on SRVoucher.vID=data_SaleReturnInfo.AccountVoucherID left join GLvDetail SrVDetail on SrVDetail.vID=SRVoucher.vID and SrVDetail.GlAccountID=gen_PartiesInfo.PartyGLID 
union all
Select ISNULL(SalesMan.SaleManInfoID,0) SaleManID,Isnull(SaleManName,'No Sales Person') as SalesMan,ISNULL(SectorName,'No Sector') SectorName,ISNULL(gen_PartiesInfo.SectorID,0) as SectorID,PartyGlid, 0 AS OpeningBalance,0 as TotalSale,0 as TotalReturn,DebitJv.Dr as DebitJV,0 as PrRecovery,0 as Purchase,0 as CreditJv from gen_PartiesInfo
left join  gen_SectorInfo on gen_SectorInfo.SectorID=gen_PartiesInfo.SectorID left join gen_SaleManInfo SalesMan on SalesMan.SaleManInfoID=gen_PartiesInfo.SaleManInfoID 
left join data_SaleInfo on data_SaleInfo.PartyID=gen_PartiesInfo.PartyID
left join GLvDetail DebitJv on DebitJv.GlAccountID=gen_PartiesInfo.PartyGLID and DebitJv.vID<>data_SaleInfo.AccountVoucherID left join glvmain debitJvMain on debitJvMain.vID=DebitJv.vID 

union all
Select ISNULL(SalesMan.SaleManInfoID,0) SaleManID,Isnull(SaleManName,'No Sales Person') as SalesMan,ISNULL(SectorName,'No Sector') SectorName,ISNULL(gen_PartiesInfo.SectorID,0) as SectorID,gen_partiesInfo.PartyGlid, 0 AS OpeningBalance,0 as TotalSale,0 as TotalReturn,0 as DebitJV,ReceoverdAmount as PrRecovery,0 as Purchase,0 as CreditJv from gen_PartiesInfo
left join  gen_SectorInfo on gen_SectorInfo.SectorID=gen_PartiesInfo.SectorID left join gen_SaleManInfo SalesMan on SalesMan.SaleManInfoID=gen_PartiesInfo.SaleManInfoID 
left join data_pes_PRVoucherInfo on data_pes_PRVoucherInfo.PartyGLID=gen_PartiesInfo.PartyGLID
left join data_Pes_PRVoucherDetail PrVoucher on PrVoucher.PRVoucherId=data_pes_PRVoucherInfo.PRVoucherId 
union all
Select ISNULL(SalesMan.SaleManInfoID,0) SaleManID,Isnull(SaleManName,'No Sales Person') as SalesMan,ISNULL(SectorName,'No Sector') SectorName,ISNULL(gen_PartiesInfo.SectorID,0) as SectorID,PartyGlid,0 AS OpeningBalance,Glvdetail.dr as TotalSale,0 as TotalReturn,0 as DebitJV,0 as PrRecovery,GLvDetail.Cr as Purchase,0 as CreditJv from gen_PartiesInfo
left join  gen_SectorInfo on gen_SectorInfo.SectorID=gen_PartiesInfo.SectorID left join gen_SaleManInfo SalesMan on SalesMan.SaleManInfoID=gen_PartiesInfo.SaleManInfoID 
left join data_PurchaseInfo on data_PurchaseInfo.PartyID=gen_PartiesInfo.PartyID
left join GLvMAIN on GLvMAIN.vID=data_PurchaseInfo.AccountVoucherID left join GLvDetail on GLvMAIN.vID=GLvDetail.vID and GLvDetail.GlAccountID=gen_PartiesInfo.PartyGLID

union all
Select ISNULL(SalesMan.SaleManInfoID,0) SaleManID,Isnull(SaleManName,'No Sales Person') as SalesMan,ISNULL(SectorName,'No Sector') SectorName,ISNULL(gen_PartiesInfo.SectorID,0) as SectorID,PartyGlid, 0 AS OpeningBalance,0 as TotalSale,0 as TotalReturn,0 as DebitJV,0 as PrRecovery,0 as Purchase,CreditJv.cr as CreditJv from gen_PartiesInfo
left join  gen_SectorInfo on gen_SectorInfo.SectorID=gen_PartiesInfo.SectorID left join gen_SaleManInfo SalesMan on SalesMan.SaleManInfoID=gen_PartiesInfo.SaleManInfoID 
left join data_PurchaseInfo on data_PurchaseInfo.PartyID=gen_PartiesInfo.PartyID
left join GLvDetail CreditJv on CreditJv.GlAccountID=gen_PartiesInfo.PartyGLID and CreditJv.vID<>data_PurchaseInfo.AccountVoucherID left join glvmain CreditJvMain on CreditJvMain.vID=CreditJv.vID 


)temp group by SaleManID,SalesMan,SectorName,SectorID


