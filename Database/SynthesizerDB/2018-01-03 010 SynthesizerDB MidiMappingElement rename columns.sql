exec sp_rename 'MidiMappingElement.NoteNumberFrom', 'FromNoteNumber', 'COLUMN'
exec sp_rename 'MidiMappingElement.NoteNumberTill', 'TillNoteNumber', 'COLUMN'
exec sp_rename 'MidiMappingElement.ControllerValueFrom', 'FromControllerValue', 'COLUMN'
exec sp_rename 'MidiMappingElement.ControllerValueTill', 'TillControllerValue', 'COLUMN'
exec sp_rename 'MidiMappingElement.VelocityValueFrom', 'FromVelocity', 'COLUMN'
exec sp_rename 'MidiMappingElement.VelocityValueTill', 'TillVelocity', 'COLUMN'
exec sp_rename 'MidiMappingElement.DimensionValueFrom', 'FromDimensionValue', 'COLUMN'
exec sp_rename 'MidiMappingElement.DimensionValueTill', 'TillDimensionValue', 'COLUMN'
exec sp_rename 'MidiMappingElement.DimensionMinValue', 'MinDimensionValue', 'COLUMN'
exec sp_rename 'MidiMappingElement.DimensionMaxValue', 'MaxDimensionValue', 'COLUMN'
exec sp_rename 'MidiMappingElement.ListIndexFrom', 'FromPosition', 'COLUMN'
exec sp_rename 'MidiMappingElement.ListIndexTill', 'TillPosition', 'COLUMN'
exec sp_rename 'MidiMappingElement.ToneIndexFrom', 'FromToneNumber', 'COLUMN'
exec sp_rename 'MidiMappingElement.ToneIndexTill', 'TillToneNumber', 'COLUMN'
