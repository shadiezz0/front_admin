export interface CodeGenerationRequest {
  codeTypeId: number;
  nameAr: string;
  nameEn: string;
  descriptionAr: string;
  descriptionEn: string;
  externalSystem: string;
  externalReferenceId: string;
}

export interface CodeItem {
  id: number;
  codeTypeId: number;
  codeTypeCode: string;
  codeTypeNameEn: string;
  nameAr: string;
  nameEn: string;
  descriptionAr: string;
  descriptionEn: string;
  codeGenerated: string;
  status: string;
  externalSystem: string;
  externalReferenceId: string;
  createdBy: string;
  createdAt: string;
  approvedAt: string | null;
  approvedBy: string | null;
}

export interface CodeGenerationResponse {
  statusCode: number;
  message: string;
  data: CodeItem;
}

export interface CodesListResponse {
  statusCode: number;
  message: string;
  data: CodeItem[];
}
