USE [Charts]
GO
/****** Object:  StoredProcedure [dbo].[GetTransactionCount]    Script Date: 01/08/2025 19:29:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetTransactionCount]
AS
BEGIN
    SET NOCOUNT ON;
    SELECT COUNT(*) AS TotalCount
    FROM [dbo].[ReceivedTransactions];
END
GO
/****** Object:  StoredProcedure [dbo].[sp_AdminLogin]    Script Date: 01/08/2025 19:29:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_AdminLogin]
    @Email VARCHAR(255),
    @Password VARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @StoredHash VARCHAR(255);
    DECLARE @InputHash VARCHAR(255);

    -- Normalize email (optional but recommended)
    SELECT @StoredHash = PasswordHash
    FROM Users
    WHERE LOWER(Email) = LOWER(@Email);

    -- Validate hash format and compare
    IF @StoredHash IS NOT NULL AND LEFT(@StoredHash, 6) = 'SHA256'
    BEGIN
        -- Compute input hash with SHA256 prefix
        SET @InputHash = 'SHA256' + CONVERT(VARCHAR(255), HASHBYTES('SHA2_256', @Password), 2);

        IF @InputHash = @StoredHash
        BEGIN
            SELECT UserId, FullName, Email, RoleId
            FROM Users
            WHERE LOWER(Email) = LOWER(@Email);
        END
        ELSE
        BEGIN
            SELECT NULL AS UserId, NULL AS FullName, NULL AS Email, NULL AS RoleId;
        END
    END
    ELSE
    BEGIN
        SELECT NULL AS UserId, NULL AS FullName, NULL AS Email, NULL AS RoleId;
    END
END;
GO
/****** Object:  StoredProcedure [dbo].[sp_CreateUser]    Script Date: 01/08/2025 19:29:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_CreateUser]
    @FullName NVARCHAR(100),
    @Email NVARCHAR(255),
    @PasswordHash NVARCHAR(255), 
    @RoleId INT
AS
BEGIN
    SET NOCOUNT ON;

    -- 1. Check for existing email
    IF EXISTS (SELECT 1 FROM Users WHERE Email = @Email)
    BEGIN
        RAISERROR('A user with this email already exists.', 16, 1);
        RETURN;
    END

    -- 2. Insert new user
    INSERT INTO Users (
        FullName,
        Email,
        PasswordHash,
        RoleId,
        CreatedDate
    )
    VALUES (
        @FullName,
        @Email,
        @PasswordHash,
        @RoleId,
        GETDATE()
    );

    -- 3. Return the new user's ID
    SELECT SCOPE_IDENTITY() AS NewUserId;
END;
GO
/****** Object:  StoredProcedure [dbo].[sp_FilterReceivedTransactions]    Script Date: 01/08/2025 19:29:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_FilterReceivedTransactions]
    @TransNo NVARCHAR(50) = NULL,
    @CustomerRef NVARCHAR(50) = NULL,
    @CustomerName NVARCHAR(100) = NULL,
    @CustomerType NVARCHAR(50) = NULL,
    @CustomerTel NVARCHAR(50) = NULL,
    @VendorTranId NVARCHAR(50) = NULL,
    @ReceiptNo NVARCHAR(50) = NULL,
    @VendorCode NVARCHAR(50) = NULL,
    @Teller NVARCHAR(50) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        SELECT 
            TranId,
            TransNo,
            CustomerRef,
            CustomerName,
            CustomerType,
            CustomerTel,
            Area,
            Tin,
            TranAmount,
            PaymentDate,
            RecordDate,
            TranType,
            PaymentType,
            VendorTranId,
            ReceiptNo,
            TranNarration,
            SmsSent,
            VendorCode,
            Teller,
            Reversal,
            Cancelled,
            Offline,
            UtilityCode,
            UtilityTranRef,
            SentToUtility,
            RegionCode,
            DistrictCode,
            VendorToken,
            ReconFileProcessed,
            Status,
            SentToVendor,
            UtilitySentDate,
            QueueTime,
            Reason
        FROM ReceivedTransactions
        WHERE 
            (@TransNo IS NULL OR TransNo LIKE '%' + @TransNo + '%')
            AND (@CustomerRef IS NULL OR CustomerRef LIKE '%' + @CustomerRef + '%')
            AND (@CustomerName IS NULL OR CustomerName LIKE '%' + @CustomerName + '%')
            AND (@CustomerType IS NULL OR CustomerType LIKE '%' + @CustomerType + '%')
            AND (@CustomerTel IS NULL OR CustomerTel LIKE '%' + @CustomerTel + '%')
            AND (@VendorTranId IS NULL OR VendorTranId LIKE '%' + @VendorTranId + '%')
            AND (@ReceiptNo IS NULL OR ReceiptNo LIKE '%' + @ReceiptNo + '%')
            AND (@VendorCode IS NULL OR VendorCode LIKE '%' + @VendorCode + '%')
            AND (@Teller IS NULL OR Teller LIKE '%' + @Teller + '%')
        ORDER BY RecordDate DESC;
    END TRY
    BEGIN CATCH
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        DECLARE @ErrorSeverity INT = ERROR_SEVERITY();
        DECLARE @ErrorState INT = ERROR_STATE();
        RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);
    END CATCH
END
GO
/****** Object:  StoredProcedure [dbo].[sp_GetAllNonAdminUsers]    Script Date: 01/08/2025 19:29:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_GetAllNonAdminUsers]
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        u.UserId,
        u.FullName,
        u.Email,
        r.RoleName
    FROM Users u
    INNER JOIN Roles r ON u.RoleId = r.RoleId
    WHERE u.RoleId != 1; -- Exclude Admin users
END;
GO
/****** Object:  StoredProcedure [dbo].[sp_GetAllReceivedTransactions]    Script Date: 01/08/2025 19:29:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_GetAllReceivedTransactions]
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        SELECT 
            TranId,
            TransNo,
            CustomerRef,
            CustomerName,
            CustomerType,
            CustomerTel,
            Area,
            Tin,
            TranAmount,
            PaymentDate,
            RecordDate,
            TranType,
            PaymentType,
            VendorTranId,
            ReceiptNo,
            TranNarration,
            SmsSent,
            VendorCode,
            Teller,
            Reversal,
            Cancelled,
            Offline,
            UtilityCode,
            UtilityTranRef,
            SentToUtility,
            RegionCode,
            DistrictCode,
            VendorToken,
            ReconFileProcessed,
            Status,
            SentToVendor,
            UtilitySentDate,
            QueueTime,
            Reason
        FROM ReceivedTransactions
        ORDER BY RecordDate DESC;
    END TRY
    BEGIN CATCH
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        DECLARE @ErrorSeverity INT = ERROR_SEVERITY();
        DECLARE @ErrorState INT = ERROR_STATE();
        RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);
    END CATCH
END
GO
/****** Object:  StoredProcedure [dbo].[sp_GetAllReceivedTransactionsByVendor]    Script Date: 01/08/2025 19:29:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[sp_GetAllReceivedTransactionsByVendor]
    @VendorCode NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        TranId, TransNo, CustomerRef, CustomerName, CustomerType, CustomerTel, Area, Tin, 
        TranAmount, PaymentDate, RecordDate, TranType, PaymentType, VendorTranId, ReceiptNo, 
        TranNarration, SmsSent, VendorCode, Teller, Reversal, Cancelled, Offline, UtilityCode, 
        UtilityTranRef, SentToUtility, RegionCode, DistrictCode, VendorToken, ReconFileProcessed, 
        Status, SentToVendor, UtilitySentDate, QueueTime, Reason
    FROM ReceivedTransactions
    WHERE VendorCode = @VendorCode;
END;
GO
/****** Object:  StoredProcedure [dbo].[sp_GetAllReceivedTransactionsOrdered]    Script Date: 01/08/2025 19:29:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_GetAllReceivedTransactionsOrdered]
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        TranId, TransNo, CustomerRef, CustomerName, CustomerType, CustomerTel, Area, Tin, 
        TranAmount, PaymentDate, RecordDate, TranType, PaymentType, VendorTranId, ReceiptNo, 
        TranNarration, SmsSent, VendorCode, Teller, Reversal, Cancelled, Offline, UtilityCode, 
        UtilityTranRef, SentToUtility, RegionCode, DistrictCode, VendorToken, ReconFileProcessed, 
        Status, SentToVendor, UtilitySentDate, QueueTime, Reason
    FROM ReceivedTransactions
    ORDER BY TranId;
END;
GO
/****** Object:  StoredProcedure [dbo].[sp_GetAllRoles]    Script Date: 01/08/2025 19:29:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_GetAllRoles]
AS
BEGIN
    SET NOCOUNT ON;

    SELECT RoleId, RoleName
    FROM Roles
    ORDER BY RoleName;
END;
GO
/****** Object:  StoredProcedure [dbo].[sp_GetCustomerTypeDistribution]    Script Date: 01/08/2025 19:29:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_GetCustomerTypeDistribution]
AS
BEGIN
    SET NOCOUNT ON;
    SELECT CustomerType, COUNT(*) as TypeCount
    FROM ReceivedTransactions
    GROUP BY CustomerType;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_GetMostRecentTransactions]    Script Date: 01/08/2025 19:29:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_GetMostRecentTransactions]
AS
BEGIN
    SELECT TOP 10 TranId, TransNo, CustomerRef, CustomerName, CustomerType, PaymentDate, VendorCode, Status
    FROM ReceivedTransactions
    ORDER BY PaymentDate DESC;
END;
GO
/****** Object:  StoredProcedure [dbo].[sp_GetStatusDistribution]    Script Date: 01/08/2025 19:29:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_GetStatusDistribution]
AS
BEGIN
    SELECT Status, COUNT(*) as StatusCount
    FROM ReceivedTransactions
    GROUP BY Status;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_GetTransactionCountsByRecordDate]    Script Date: 01/08/2025 19:29:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_GetTransactionCountsByRecordDate]
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        SELECT 
            CAST(RecordDate AS DATE) AS RecordDate, 
            COUNT(*) AS TransactionCount
        FROM ReceivedTransactions
        GROUP BY CAST(RecordDate AS DATE)
        ORDER BY CAST(RecordDate AS DATE);
    END TRY
    BEGIN CATCH
        DECLARE @ErrorMessage NVARCHAR(4000);
        DECLARE @ErrorSeverity INT;
        DECLARE @ErrorState INT;
        
        SET @ErrorMessage = ERROR_MESSAGE();
        SET @ErrorSeverity = ERROR_SEVERITY();
        SET @ErrorState = ERROR_STATE();
        
        RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);
    END CATCH
END
GO
/****** Object:  StoredProcedure [dbo].[sp_GetTransactionCountsByRecordDateAndVendor]    Script Date: 01/08/2025 19:29:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_GetTransactionCountsByRecordDateAndVendor]
    @VendorCode NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        CAST(RecordDate AS DATE) AS RecordDate, 
        COUNT(*) AS TransactionCount
    FROM ReceivedTransactions
    WHERE VendorCode = @VendorCode
    GROUP BY CAST(RecordDate AS DATE)
    ORDER BY RecordDate;
END;
GO
/****** Object:  StoredProcedure [dbo].[sp_GetTransactionCountsByVendorCode]    Script Date: 01/08/2025 19:29:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_GetTransactionCountsByVendorCode]
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        VendorCode,
        COUNT(*) AS TransactionCount
    FROM 
        ReceivedTransactions
    WHERE 
        VendorCode IS NOT NULL
        AND VendorCode <> ''
    GROUP BY 
        VendorCode
    ORDER BY 
        VendorCode;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_GetTransactionsPerHour]    Script Date: 01/08/2025 19:29:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_GetTransactionsPerHour]
AS
BEGIN
    SELECT DATEPART(HOUR, RecordDate) AS HourOfDay, COUNT(*) AS TransactionCount
    FROM ReceivedTransactions
    GROUP BY DATEPART(HOUR, RecordDate)
    ORDER BY HourOfDay;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_InsertReceivedTransaction]    Script Date: 01/08/2025 19:29:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_InsertReceivedTransaction]
    @TransNo VARCHAR(50),
    @CustomerRef VARCHAR(50),
    @CustomerName VARCHAR(100),
    @CustomerType VARCHAR(50),
    @CustomerTel VARCHAR(50),
    @Area VARCHAR(50),
    @Tin VARCHAR(50),
    @TranAmount MONEY,
    @PaymentDate DATETIME,
    @RecordDate DATETIME,
    @TranType VARCHAR(50),
    @PaymentType VARCHAR(50),
    @VendorTranId VARCHAR(100),
    @ReceiptNo VARCHAR(50),
    @TranNarration VARCHAR(300),
    @SmsSent BIT,
    @VendorCode VARCHAR(50),
    @Teller VARCHAR(50),
    @Reversal BIT = NULL,
    @Cancelled BIT = NULL,
    @Offline BIT = NULL,
    @UtilityCode VARCHAR(50),
    @UtilityTranRef VARCHAR(100) = NULL,
    @SentToUtility BIT,
    @RegionCode VARCHAR(50) = NULL,
    @DistrictCode VARCHAR(50) = NULL,
    @VendorToken VARCHAR(50) = NULL,
    @ReconFileProcessed BIT = NULL,
    @Status VARCHAR(50),
    @SentToVendor INT = NULL,
    @UtilitySentDate DATETIME = NULL,
    @QueueTime DATETIME = NULL,
    @Reason VARCHAR(100) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        INSERT INTO ReceivedTransactions (
            TransNo, CustomerRef, CustomerName, CustomerType, CustomerTel, Area, Tin,
            TranAmount, PaymentDate, RecordDate, TranType, PaymentType, VendorTranId,
            ReceiptNo, TranNarration, SmsSent, VendorCode, Teller, Reversal, Cancelled, Offline,
            UtilityCode, UtilityTranRef, SentToUtility, RegionCode, DistrictCode, VendorToken,
            ReconFileProcessed, Status, SentToVendor, UtilitySentDate, QueueTime, Reason
        )
        VALUES (
            @TransNo, @CustomerRef, @CustomerName, @CustomerType, @CustomerTel, @Area, @Tin,
            @TranAmount, @PaymentDate, @RecordDate, @TranType, @PaymentType, @VendorTranId,
            @ReceiptNo, @TranNarration, @SmsSent, @VendorCode, @Teller, @Reversal, @Cancelled, @Offline,
            @UtilityCode, @UtilityTranRef, @SentToUtility, @RegionCode, @DistrictCode, @VendorToken,
            @ReconFileProcessed, @Status, @SentToVendor, @UtilitySentDate, @QueueTime, @Reason
        );
    END TRY
    BEGIN CATCH
        -- Log error or return error message
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        DECLARE @ErrorSeverity INT = ERROR_SEVERITY();
        DECLARE @ErrorState INT = ERROR_STATE();
        RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);
    END CATCH
END



GO
