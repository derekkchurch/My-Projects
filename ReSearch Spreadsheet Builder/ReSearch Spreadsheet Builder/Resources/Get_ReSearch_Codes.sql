/*
******************************************************************
*** SEE BELOW FOR INSTRUCTIONS                                 ***
*** Your request must be exported in a tab delimited format.   *** 
******************************************************************
*/
SET NOCOUNT ON
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

--DECLARE @SiteID AS CHAR(36)
--SET @SiteID = 'CONV-TXDENTON'

DECLARE @OrgChartXML AS XML,
        @NodeIDs AS VARCHAR(MAX),
        @CaseCategoryKeys AS VARCHAR(MAX),
        @CaseTypeCodes AS VARCHAR(MAX),
        @CaseEventTypeCodes AS VARCHAR(MAX)

SET @NodeIDs = '' -- comma-delimited NodeIDs, or leave blank for all nodes
SET @CaseCategoryKeys = ''
SET @CaseTypeCodes = ''
SET @CaseEventTypeCodes = ''


DECLARE @OrgChart AS TABLE
(  ParentNodeID INT NULL, 
   NodeID INT NOT NULL,
   NodeName VARCHAR(MAX) NULL,
   NodeInformation XML NOT NULL  )

DECLARE @OrgChartHierarchy AS TABLE
(  ParentNodeID INT NULL, 
   NodeID INT NOT NULL,
   NodeName VARCHAR(MAX) NULL,
   LeafNodeID INT NOT NULL,
   LeafNodeName VARCHAR(MAX) NULL,
   NodeLevel INT NOT NULL  )

-- Location (ORG)
SELECT NULL AS 'Location Nodes (ORG)', NodeIDParent AS 'Parent Node ID', NodeID AS 'Node ID', OrgUnitName AS 'Node Name'
FROM Operations.dbo.fnGetNodeList(@SiteID)
WHERE NodeIDProduct IN (1,12)

-- *************
SELECT @OrgChartXML = OrgChartXml
FROM Operations.dbo.[Site]
WHERE [GUID] = @SiteID;

INSERT @OrgChart (ParentNodeID, NodeID, NodeName, NodeInformation)
SELECT OrgChart.Node.value('(./../@ID)[1]', 'VARCHAR(MAX)') AS ParentNodeID,
	  OrgChart.Node.value('(@ID)[1]', 'VARCHAR(MAX)') AS NodeID,
	  OrgUnits.OrgUnit.value('(@Name)[1]', 'VARCHAR(MAX)') AS NodeName,
	  OrgChart.Node.query('.') AS NodeInformation
FROM @OrgChartXML.nodes('/OrgMap/OrgChart//Node') OrgChart(Node)
CROSS APPLY @OrgChartXML.nodes('/OrgMap/OrgUnits/OrgUnit') OrgUnits(OrgUnit)
WHERE OrgChart.Node.value('(@OrgUnitID)[1]', 'VARCHAR(MAX)') = OrgUnits.OrgUnit.value('(@ID)[1]', 'VARCHAR(MAX)')

IF LEN(@NodeIDs) = 0
BEGIN
   ;WITH OrgChart_CaseManager (ParentNodeID, NodeID, NodeName, NodeLevel) AS (
	 SELECT ParentNodeID, NodeID, NodeName, 0 AS NodeLevel FROM @OrgChart WHERE NodeID = 1 -- Start at Case Manager and walk the tree downwards
	 UNION ALL
	 SELECT OC.ParentNodeID, OC.NodeID, OC.NodeName, OCCM.NodeLevel + 1 AS NodeLevel FROM @OrgChart OC INNER JOIN OrgChart_CaseManager OCCM ON OC.ParentNodeID = OCCM.NodeID)

   SELECT @NodeIDs = @NodeIDs + CAST(NodeID AS VARCHAR(128)) + ','
   FROM OrgChart_CaseManager
   ORDER BY NodeID;
END

---- Court Location  (moved from above setting NodeIDs)
--;WITH OrgChart_CaseManager (ParentNodeID, NodeID, NodeName, NodeLevel) AS (
--	 SELECT ParentNodeID, NodeID, NodeName, 0 AS NodeLevel FROM @OrgChart WHERE NodeID = 1 -- Start at Case Manager and walk the tree downwards
--	 UNION ALL
--	 SELECT OC.ParentNodeID, OC.NodeID, OC.NodeName, OCCM.NodeLevel + 1 AS NodeLevel FROM @OrgChart OC INNER JOIN OrgChart_CaseManager OCCM ON OC.ParentNodeID = OCCM.NodeID)

--SELECT DISTINCT 'Court Location' AS 'Court Location', OCCM.ParentNodeID, OCCM.NodeID, OCCM.NodeName 
--FROM OrgChart_CaseManager OCCM
--INNER JOIN Justice.dbo.fnKeyValuesVarCharSep(@NodeIDs, ',') KV ON OCCM.NodeID = KV.KeyValue
--ORDER BY OCCM.NodeID;

IF LEN(@CaseCategoryKeys) = 0
   SELECT @CaseCategoryKeys = @CaseCategoryKeys + CaseCategoryKey + ',' FROM Justice.dbo.sCaseCat WHERE ProductIDs = 2;

IF LEN(@CaseTypeCodes) = 0
   SELECT @CaseTypeCodes = @CaseTypeCodes + Code + ',' FROM Justice.dbo.uCode WHERE CacheTableID = 91;
				  
IF LEN(@CaseEventTypeCodes) = 0
   SELECT @CaseEventTypeCodes = @CaseEventTypeCodes + Code + ',' FROM Justice.dbo.uCode WHERE CacheTableID = 59;

IF LEN(@NodeIDs) > 0 AND RIGHT(@NodeIDs, 1) = ','
   SET @NodeIDs = LEFT(@NodeIDs, LEN(@NodeIDs) - 1);

IF LEN(@CaseCategoryKeys) > 0 AND RIGHT(@CaseCategoryKeys, 1) = ','
   SET @CaseCategoryKeys = LEFT(@CaseCategoryKeys, LEN(@CaseCategoryKeys) - 1);

IF LEN(@CaseTypeCodes) > 0 AND RIGHT(@CaseTypeCodes, 1) = ','
   SET @CaseTypeCodes = LEFT(@CaseTypeCodes, LEN(@CaseTypeCodes) - 1);

IF LEN(@CaseEventTypeCodes) > 0 AND RIGHT(@CaseEventTypeCodes, 1) = ','
   SET @CaseEventTypeCodes = LEFT(@CaseEventTypeCodes, LEN(@CaseEventTypeCodes) - 1);

-- Build org chart parent hierarchy to traverse when determining inheritance
;WITH OrgChartParent (ParentNodeID, NodeID, NodeName, LeafNodeID, LeafNodeName, NodeLevel) AS (
	 SELECT OC.ParentNodeID, OC.NodeID, OC.NodeName, OC.NodeID, OC.NodeName, 0 AS NodeLevel FROM @OrgChart OC INNER JOIN Justice.dbo.fnKeyValuesVarCharSep(@NodeIDs, ',') KV ON OC.NodeID = KV.KeyValue -- WHERE    OC.NodeID = @NodeID -- Start at the node and walk upwards
	 UNION ALL
	 SELECT OC.ParentNodeID, OC.NodeID, OC.NodeName, OCP.LeafNodeID, OCP.LeafNodeName, OCP.NodeLevel + 1 AS NodeLevel FROM @OrgChart OC INNER JOIN OrgChartParent OCP ON OC.NodeID = OCP.ParentNodeID)

INSERT INTO @OrgChartHierarchy (ParentNodeID, NodeID, NodeName, LeafNodeID, LeafNodeName, NodeLevel)
SELECT DISTINCT ParentNodeID, NodeID, NodeName, LeafNodeID, LeafNodeName, NodeLevel FROM OrgChartParent;

-- Case Type (CT)
SELECT DISTINCT NULL AS 'Case Type (CT)', OCH.LeafNodeID AS 'Node ID', uC.Code AS 'Code Word', uC.Description AS 'Description'   --, sCT.CaseCategoryKey AS 'Case Category Reference Code'
FROM Justice.dbo.uCode uC
INNER JOIN Justice.dbo.xuCodeNode xuCN ON uC.CodeID = xuCN.CodeID
INNER JOIN Justice.dbo.uCaseType uCT ON uC.CodeID = uCT.CaseUTypeID
INNER JOIN Justice.dbo.uCaseTypeNode uCTN ON uC.CodeID = uCTN.CaseUTypeID AND xuCN.NodeID = uCTN.NodeID
INNER JOIN Justice.dbo.sCaseType sCT ON uCT.CaseTypeID = sCT.CaseTypeID
INNER JOIN (SELECT uC_INNER.CodeID, OCH_INNER.LeafNodeID, MIN(OCH_INNER.NodeLevel) AS NodeLevel
            FROM Justice.dbo.uCode uC_INNER
            INNER JOIN Justice.dbo.xuCodeNode xuCN_INNER on uC_INNER.CodeID = xuCN_INNER.CodeID
            INNER JOIN @OrgChartHierarchy OCH_INNER ON xuCN_INNER.NodeID = OCH_INNER.NodeID
            WHERE uC_INNER.CacheTableID = 91
            GROUP BY uC_INNER.CodeID, OCH_INNER.LeafNodeID) NodeLevel ON uC.CodeID = NodeLevel.CodeID
INNER JOIN @OrgChartHierarchy OCH ON NodeLevel.NodeLevel = OCH.NodeLevel AND NodeLevel.LeafNodeID = OCH.LeafNodeID AND xuCN.NodeID = OCH.NodeID
INNER JOIN Justice.dbo.fnKeyValuesVarCharSep(@CaseCategoryKeys, ',') KV_CCK ON sCT.CaseCategoryKey = KV_CCK.KeyValue
INNER JOIN Justice.dbo.fnKeyValuesVarCharSep(@CaseTypeCodes, ',') KV_CTC ON uC.Code = KV_CTC.KeyValue
WHERE uC.CacheTableID = 91 AND uC.RootNodeID = 1 AND ISNULL(uC.ObsoleteDate, GETDATE() + 1) > GETDATE() AND ISNULL(uC.EffectiveDate, GETDATE() - 1) < GETDATE() AND xuCN.Hidden = 0
ORDER BY OCH.LeafNodeID, uC.Code, uC.Description --, sCT.CaseCategoryKey

-- Case Security Group (CSG)
SELECT DISTINCT NULL AS 'Case Security Group (CSG)', OCH.LeafNodeID AS 'Node ID', uC.Code AS 'Code Word', uC.Description AS 'Description'
FROM Justice.dbo.uCode uC
INNER JOIN Justice.dbo.xuCodeNode xuCN ON uC.CodeID = xuCN.CodeID
INNER JOIN (SELECT uC_INNER.CodeID, OCH_INNER.LeafNodeID, MIN(OCH_INNER.NodeLevel) AS NodeLevel
            FROM Justice.dbo.uCode uC_INNER
            INNER JOIN Justice.dbo.xuCodeNode xuCN_INNER on uC_INNER.CodeID = xuCN_INNER.CodeID
            INNER JOIN @OrgChartHierarchy OCH_INNER ON xuCN_INNER.NodeID = OCH_INNER.NodeID
            WHERE uC_INNER.CacheTableID = 57
            GROUP BY uC_INNER.CodeID, OCH_INNER.LeafNodeID) NodeLevel ON uC.CodeID = NodeLevel.CodeID
INNER JOIN @OrgChartHierarchy OCH ON NodeLevel.NodeLevel = OCH.NodeLevel AND NodeLevel.LeafNodeID = OCH.LeafNodeID AND xuCN.NodeID = OCH.NodeID
WHERE uC.CacheTableID = 57 AND uC.RootNodeID = 1 AND ISNULL(uC.ObsoleteDate, GETDATE() + 1) > GETDATE() AND ISNULL(uC.EffectiveDate, GETDATE() - 1) < GETDATE() AND xuCN.Hidden = 0
ORDER BY OCH.LeafNodeID, uC.Code, uC.Description

-- Document Type (DT)
SELECT DISTINCT NULL AS 'Document Type', OCH.LeafNodeID AS 'Node ID', uC.Code AS 'Code Word', uC.Description AS 'Description'
FROM Operations.dbo.uCode uC
INNER JOIN Operations.dbo.xuCodeNode xuCN ON uC.CodeID = xuCN.CodeID
INNER JOIN Operations.dbo.uDocType uDT ON uC.CodeID = uDT.DocumentTypeID
INNER JOIN Operations.dbo.uDocTypeNode uDTN ON uC.CodeID = uDTN.DocumentTypeID AND xuCN.NodeID = uDTN.NodeID
INNER JOIN (SELECT uC_INNER.CodeID, OCH_INNER.LeafNodeID, MIN(OCH_INNER.NodeLevel) AS NodeLevel
            FROM Operations.dbo.uCode uC_INNER
            INNER JOIN Operations.dbo.xuCodeNode xuCN_INNER on uC_INNER.CodeID = xuCN_INNER.CodeID
            INNER JOIN @OrgChartHierarchy OCH_INNER ON xuCN_INNER.NodeID = OCH_INNER.NodeID
            WHERE uC_INNER.CacheTableID = 15
            GROUP BY uC_INNER.CodeID, OCH_INNER.LeafNodeID) NodeLevel ON uC.CodeID = NodeLevel.CodeID
INNER JOIN @OrgChartHierarchy OCH ON NodeLevel.NodeLevel = OCH.NodeLevel AND NodeLevel.LeafNodeID = OCH.LeafNodeID AND xuCN.NodeID = OCH.NodeID
--LEFT OUTER JOIN Operations.dbo.xuDocTypesMergeCat xuDTsMC ON uC.CodeID = xuDTsMC.DocumentTypeID
WHERE uC.CacheTableID = 15 AND uC.RootNodeID = 0 AND ISNULL(uC.ObsoleteDate, GETDATE() + 1) > GETDATE() AND ISNULL(uC.EffectiveDate, GETDATE() - 1) < GETDATE() AND xuCN.Hidden = 0    -- AND uDT.DocumentCategoryID = 2 AND xuDTsMC.DocumentTypeID IS NULL ???
GROUP BY OCH.LeafNodeID, OCH.LeafNodeName, uC.Code, uC.Description

-- Document Security Group (DSG)
SELECT DISTINCT NULL AS 'Document Security Group (DSG)', OCH.LeafNodeID AS 'Node ID', uC.Code AS 'Code Word', uC.Description AS 'Description'
FROM Operations.dbo.uCode uC
INNER JOIN Operations.dbo.xuCodeNode xuCN ON uC.CodeID = xuCN.CodeID
INNER JOIN (SELECT uC_INNER.CodeID, OCH_INNER.LeafNodeID, MIN(OCH_INNER.NodeLevel) AS NodeLevel
            FROM Operations.dbo.uCode uC_INNER
            INNER JOIN Operations.dbo.xuCodeNode xuCN_INNER on uC_INNER.CodeID = xuCN_INNER.CodeID
            INNER JOIN @OrgChartHierarchy OCH_INNER ON xuCN_INNER.NodeID = OCH_INNER.NodeID
            WHERE uC_INNER.CacheTableID = 88
            GROUP BY uC_INNER.CodeID, OCH_INNER.LeafNodeID) NodeLevel ON uC.CodeID = NodeLevel.CodeID
INNER JOIN @OrgChartHierarchy OCH ON NodeLevel.NodeLevel = OCH.NodeLevel AND NodeLevel.LeafNodeID = OCH.LeafNodeID AND xuCN.NodeID = OCH.NodeID
WHERE uC.CacheTableID = 88 AND uC.RootNodeID = 0 AND ISNULL(uC.ObsoleteDate, GETDATE() + 1) > GETDATE() AND ISNULL(uC.EffectiveDate, GETDATE() - 1) < GETDATE() AND xuCN.Hidden = 0
GROUP BY OCH.LeafNodeID, OCH.LeafNodeName, uC.Code, uC.Description

-- Extended Connections
SELECT DISTINCT NULL AS 'Extended Connections', OCH.LeafNodeID AS 'Node ID', uC.Code AS 'Code Word', uC.Description AS 'Description'
FROM Justice.dbo.uCode uC
INNER JOIN Justice.dbo.xuCodeNode xuCN ON uC.CodeID = xuCN.CodeID
INNER JOIN (SELECT uC_INNER.CodeID, OCH_INNER.LeafNodeID, MIN(OCH_INNER.NodeLevel) AS NodeLevel
            FROM Justice.dbo.uCode uC_INNER
            INNER JOIN Justice.dbo.xuCodeNode xuCN_INNER on uC_INNER.CodeID = xuCN_INNER.CodeID
            INNER JOIN @OrgChartHierarchy OCH_INNER ON xuCN_INNER.NodeID = OCH_INNER.NodeID
            WHERE uC_INNER.CacheTableID = 96
            GROUP BY uC_INNER.CodeID, OCH_INNER.LeafNodeID) NodeLevel ON uC.CodeID = NodeLevel.CodeID
INNER JOIN @OrgChartHierarchy OCH ON NodeLevel.NodeLevel = OCH.NodeLevel AND NodeLevel.LeafNodeID = OCH.LeafNodeID AND xuCN.NodeID = OCH.NodeID
WHERE uC.CacheTableID = 96 AND uC.RootNodeID = 1 AND ISNULL(uC.ObsoleteDate, GETDATE() + 1) > GETDATE() AND ISNULL(uC.EffectiveDate, GETDATE() - 1) < GETDATE() AND xuCN.Hidden = 0
ORDER BY OCH.LeafNodeID, uC.Code
