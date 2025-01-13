----------
-- Get All
----------

create procedure sp_GetAllProducts
as
begin

	select ProductID, ProductName, Price, Qty, Remarks from dbo.tbl_ProductMaster with(nolock) 
	-- nolock, helps to extract data in case any deadlock happens

end


---------
-- Insert
---------

alter proc sp_InsertProducts
(
@ProductName nvarchar(50),
@Price decimal(8,2), -- 8 digit, 2 decimal
@Qty int,
@Remarks nvarchar(50) = NULL
)
as
begin
declare @RowCount int = 0
set @RowCount = (select count(1) from dbo.tbl_ProductMaster where ProductName = @ProductName)
-- validating if product exists or not, to avoid inserting duplicate product

	begin try
		begin tran

		if(@RowCount = 0)
		begin
				insert into dbo.tbl_ProductMaster (ProductName, Price, Qty, Remarks)
				values (@ProductName, @Price, @Qty, @Remarks)
			end
		commit tran
	end try

	begin catch
		rollback tran
		select ERROR_MESSAGE()
	end catch
end


--------------
-- Edit/Update
--------------

-- 2 sproc needed
-- 1st GetProductByID
create proc sp_GetProductByID
(
@ProductID int
)
as
begin
	select ProductID, ProductName, Price, Qty, Remarks from dbo.tbl_ProductMaster 
	where ProductID = @ProductID
end

--exec sp_GetProductByID 3


-- 2nd Update Product

create proc sp_UpdateProducts
(
@ProductID int,
@ProductName nvarchar(50),
@Price decimal(8,2), 
@Qty int,
@Remarks nvarchar(50) = NULL
)
as
begin
declare @RowCount int = 0
set @RowCount = (select count(1) from dbo.tbl_ProductMaster where ProductName = @ProductName and ProductID <> @ProductID)
-- validating if product exists or not, to avoid inserting duplicate product and if product already available don't update. Only update unavailable productname
-- In short, logic is do not insert or update duplicate value with the same ID.

	begin try
		begin tran

		if(@RowCount = 0)
		begin
				update dbo.tbl_ProductMaster set
					ProductName		= @ProductName,
					Price			= @Price,
					Qty				= @Qty,
					Remarks			= @Remarks
				where ProductID		= @ProductID
				 
			end
		commit tran
	end try

	begin catch
		rollback tran
		select ERROR_MESSAGE()
	end catch
end



----------
-- Delete
----------

-- Validating if product exists in the table with the ID or not, if available then count will be > 0, then delete & show success message; otherwise show output message
ALTER proc [dbo].[sp_DeleteProducts]
(
@PRODUCTID int,
@OUTPUTMESSAGE varchar(50) output
)
as
begin

declare @rowcount int = 0;

	begin try

	set @rowcount = (select count(1) from dbo.tbl_ProductMaster where ProductID = @PRODUCTID)

	if(@rowcount > 0)
		begin
			begin tran
				delete from dbo.tbl_ProductMaster 
				where ProductID = @PRODUCTID

				set @OUTPUTMESSAGE = 'Product deleted successfully...!'
			commit tran
		end
	else
		begin
			set @OUTPUTMESSAGE = 'Product not available with id ' + CONVERT(varchar, @PRODUCTID)
		end

	end try

begin catch
	rollback tran
	set @OUTPUTMESSAGE = ERROR_MESSAGE()
end catch

end


