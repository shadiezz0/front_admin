# 🔄 Complete Code Generator Workflow Implementation Plan

## Overview
This document outlines the complete workflow for the code generator system with all 7 steps.

---

## 📋 Workflow Steps

### ✅ Step 1: Code Type (COMPLETED)
- **API**: `POST /api/CodeTypes/Create`
- **Fields**: codeTypeCode, nameAr, nameEn, descriptionAr, descriptionEn
- **Stores**: codeTypeId, codeTypeCode
- **Status**: ✅ Implemented

### ✅ Step 2: Code Type Attribute (COMPLETED - NEEDS UPDATE)
- **API**: `POST /api/CodeAttributeTypes/Create`
- **Current**: Single entry
- **Required**: **3 ENTRIES** - User enters form 3 times
- **Fields**: nameAr, nameEn, descriptionAr, descriptionEn
- **Stores**: Array of 3 codeAttributeTypeIds
- **Status**: ⚠️ Needs update to allow 3 entries

### 🔲 Step 3: Code Type Main (TO IMPLEMENT)
- **API**: `POST /api/CodeAttributeMains/Create`
- **Entries**: **3 ENTRIES** (one for each attribute type)
- **Fields**: code, nameAr, nameEn, descriptionAr, descriptionEn
- **Auto-filled (hidden)**: 
  - `codeTypeId` (from Step 1)
  - `codeAttributeTypeId` (from Step 2 - use index)
- **Stores**: Array of 3 code AttributeMainIds
- **Navigate to**: Code Details

### 🔲 Step 4: Code Details (TO IMPLEMENT)
- **API**: `POST /api/CodeAttributeDetails/Create`
- **Entries**: **3 ENTRIES** (one for each main)
- **Fields**: code, nameAr, nameEn, descriptionAr, descriptionEn, parentDetailId (optional), sortOrder
- **Auto-filled (hidden)**:
  - `attributeMainId` (from Step 3 - use index)
- **Stores**: Array of 3 codeAttributeDetailIds
- **Navigate to**: Code Settings

### 🔲 Step 5: Code Settings (TO IMPLEMENT)
- **API**: `POST /api/CodeTypeSettings/Create` (x3)
- **Entries**: **3 ENTRIES** (automatically created)
- **Display**: Read-only inputs showing the created details
- **Fields (readonly)**:
  - attributeDetailId (from Step 4)
  - sortOrder (1, 2, 3)
  - separator ("-")
  - isRequired (true)
- **Preview**: Show concatenated code: "XYZ-ABC-VBV"
- **Auto-filled (hidden)**:
  - `codeTypeId` (from Step 1)
- **Navigate to**: Code Sequence

### 🔲 Step 6: Code Sequence (TO IMPLEMENT)
- **API**: `POST /api/CodeTypeSequences/Create`
- **Entries**: 1 ENTRY
- **Fields**: nameEn, startWith, minValue, maxValue, currentValue, isCycling
- **Auto-filled (hidden)**:
  - `codeTypeId` (from Step 1)
- **Behavior**: 
  - Show success message with created sequence
  - Wait 30 seconds
  - Auto-navigate to Code Generation
- **Navigate to**: Code Generation (after 30s)

### 🔲 Step 7: Code Generation (TO IMPLEMENT)
- **API**: `POST /api/Codes`
- **Entries**: 1 ENTRY
- **Fields**: nameAr, nameEn, descriptionAr, descriptionEn, externalSystem, externalReferenceId
- **Auto-filled (hidden)**:
  - `codeTypeId` (from Step 1)
- **Button**: "Generate" (not "Save")
- **Response**: Show `codeGenerated` in creative way
  - Example: "XYZ-ABC-VBV-000121"
- **Display**: Animated reveal of generated code

---

## 🔧 Implementation Details

### Code Type Attribute (Update Required)

```typescript
// Multiple entries component
entries = [
  { nameAr: '', nameEn: '', descriptionAr: '', descriptionEn: '' },
  { nameAr: '', nameEn: '', descriptionAr: '', descriptionEn: '' },
  { nameAr: '', nameEn: '', descriptionAr: '', descriptionEn: '' }
];

currentEntry = 0;
savedIds: number[] = [];

onSubmit() {
  // Save current entry
  this.api.create(this.entries[this.currentEntry]).subscribe(response => {
    this.savedIds.push(response.data.id);
    
    if (this.currentEntry < 2) {
      // Move to next entry
      this.currentEntry++;
      this.resetForm();
    } else {
      // All 3 saved - store IDs and navigate
      this.savedIds.forEach(id => this.service.addCodeAttributeTypeId(id));
      this.service.completeStep(1);
      this.router.navigate(['/code-type-main']);
    }
  });
}
```

### Code Type Main

```typescript
// Form structure
form = {
  code: '',
  nameAr: '',
  nameEn: '',
  descriptionAr: '',
  descriptionEn: '',
  // Auto-filled
  codeTypeId: this.codeGenService.getState().codeTypeId,
  codeAttributeTypeId: this.codeGenService.getState().codeAttributeTypeIds[currentIndex]
};

// Save 3 times pattern (same as Code Type Attribute)
```

### Code Details

```typescript
// Similar pattern - 3 entries
// attributeMainId auto-filled from previous step
```

### Code Settings

```typescript
// Read-only display
settings = [
  { detail: 'XYZ', sortOrder: 1, separator: '-', isRequired: true },
  { detail: 'ABC', sortOrder: 2, separator: '-', isRequired: true },
  { detail: 'VBV', sortOrder: 3, separator: '-', isRequired: true }
];

generatedPattern = 'XYZ-ABC-VBV';

// Auto-create settings on component init
ngOnInit() {
  this.createAllSettings();
}
```

### Code Sequence

```typescript
// Single form
onSubmit() {
  this.api.create({
    ...this.form.value,
    codeTypeId: this.codeGenService.getState().codeTypeId
  }).subscribe(response => {
    this.showSuccess(response.data);
    
    // Wait 30 seconds then navigate
    setTimeout(() => {
      this.router.navigate(['/code-generation']);
    }, 30000);
  });
}
```

### Code Generation

```typescript
// Final step
onGenerate() {
  this.api.create({
    ...this.form.value,
    codeTypeId: this.codeGenService.getState().codeTypeId
  }).subscribe(response => {
    // Show generated code with animation
    this.generatedCode = response.data.codeGenerated;
    this.animate Reveal();
  });
}
```

---

## 🎯 Key Points

1. **Step 2, 3, 4**: Each requires 3 entries
2. **Auto-filled IDs**: codeTypeId (from Step 1), codeAttributeTypeId, attributeMainId
3. **Step 5**: Readonly display with pattern preview
4. **Step 6**: 30-second delay before navigation
5. **Step 7**: "Generate" button instead of "Save"

---

## 📊 Data Flow

```
Step 1: Code Type
  └─ Creates: codeTypeId, codeTypeCode
      
Step 2: Code Attribute Type (x3)
  └─ Creates: [id1, id2, id3]
      
Step 3: Code Type Main (x3)
  ├─ Uses: codeTypeId, codeAttributeTypeIds[0,1,2]
  └─ Creates: [mainId1, mainId2, mainId3]
      
Step 4: Code Details (x3)
  ├─ Uses: attributeMainIds[0,1,2]
  └─ Creates: [detailId1, detailId2, detailId3]
      
Step 5: Code Settings (x3)
  ├─ Uses: codeTypeId, detailIds[0,1,2]
  └─ Creates: Read-only display + pattern
      
Step 6: Code Sequence
  ├─ Uses: codeTypeId
  └─ Creates: sequence
      
Step 7: Code Generation
  ├─ Uses: codeTypeId
  └─ Creates: FINAL CODE (XYZ-ABC-VBV-000121)
```

---

## 🚀 Implementation Priority

1. ✅ Update CodeGeneratorService (add ID arrays)
2. ⚠️ Update Code Type Attribute (3 entries)
3. 🔲 Implement Code Type Main (3 entries)
4. 🔲 Implement Code Details (3 entries)
5. 🔲 Implement Code Settings (readonly display)
6. 🔲 Implement Code Sequence (30s delay)
7. 🔲 Implement Code Generation (final reveal)

---

**This is a complex multi-step workflow. Each component builds on data from previous steps.**
