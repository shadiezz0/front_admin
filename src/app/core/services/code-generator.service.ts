import { Injectable, signal } from '@angular/core';

export interface CodeGeneratorState {
    currentStep: number;
    completedSteps: boolean[];
    codeTypeId?: number;
    codeTypeCode?: string;
    codeAttributeTypeIds: number[];
    codeAttributeMainIds: number[];
    codeAttributeDetailIds: number[];
}

@Injectable({
    providedIn: 'root'
})
export class CodeGeneratorService {
    private state = signal<CodeGeneratorState>({
        currentStep: 0,
        completedSteps: [false, false, false, false, false, false, false],
        codeAttributeTypeIds: [],
        codeAttributeMainIds: [],
        codeAttributeDetailIds: []
    });

    getState() {
        return this.state();
    }

    completeStep(stepIndex: number) {
        const currentState = this.state();
        const newCompleted = [...currentState.completedSteps];
        newCompleted[stepIndex] = true;

        this.state.set({
            ...currentState,
            completedSteps: newCompleted,
            currentStep: stepIndex + 1
        });
    }

    setCodeTypeData(id: number, code: string) {
        this.state.update(s => ({
            ...s,
            codeTypeId: id,
            codeTypeCode: code
        }));
    }

    addCodeAttributeTypeId(id: number) {
        this.state.update(s => ({
            ...s,
            codeAttributeTypeIds: [...s.codeAttributeTypeIds, id]
        }));
    }

    addCodeAttributeMainId(id: number) {
        this.state.update(s => ({
            ...s,
            codeAttributeMainIds: [...s.codeAttributeMainIds, id]
        }));
    }

    addCodeAttributeDetailId(id: number) {
        this.state.update(s => ({
            ...s,
            codeAttributeDetailIds: [...s.codeAttributeDetailIds, id]
        }));
    }

    isStepCompleted(stepIndex: number): boolean {
        return this.state().completedSteps[stepIndex];
    }

    canAccessStep(stepIndex: number): boolean {
        if (stepIndex === 0) return true;
        return this.state().completedSteps[stepIndex - 1];
    }

    reset() {
        this.state.set({
            currentStep: 0,
            completedSteps: [false, false, false, false, false, false, false],
            codeAttributeTypeIds: [],
            codeAttributeMainIds: [],
            codeAttributeDetailIds: []
        });
    }
}
