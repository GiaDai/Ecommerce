export interface IProductAttributeMapping {
  Id: number;
  ProductId: number;
  ProductAttributeId: number;
  TextPrompt: string;
  IsRequired: boolean;
  AttributeControlTypeId: number;
  DisplayOrder: number;
  ValidationMinLength?: number;
  ValidationMaxLength?: number;
  ValidationFileAllowedExtensions: string;
  ValidationFileMaximumSize?: number;
  DefaultValue: string;
  ConditionAttributeXml: string;
  CreatedBy: string;
  Created: string;
  LastModifiedBy: string;
  LastModified: string;
}
