export interface CodeGenerationRequest {
  nameAr: string;
  nameEn: string;
  descriptionAr: string;
  descriptionEn: string;
  externalSystem: string;
  externalReferenceId: string;
  codeTypeId: number;
}

export interface CodeGenerationResponse {
  data: {
    codeGenerated: string;
  };
}
