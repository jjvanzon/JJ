
        /// <summary>
        /// Systematically tests all the IsUnique methods with variable amounts of arguments,
        /// to solely check if the overloads will pass the arguments correctly to the base method.
        /// </summary>
        /*[TestMethod]
        public void Test_Validate_IsUnique_VariableAmountOfArguments1()
        {
            // TODO: When it tests the IsUnique overload with 8 key fields it is very inefficient.
            // In theory, you would only have to check if switching one of the key values makes a difference,
            // Yet it checks each combination of key values.
            // If you do it more efficiently, you can not use just 1's and 0's as key values,
            // but you would have to give each key field its own set of values,
            // because otherwise you would not be testing if the methods pass the arguments in the correct order.

            int[] item1;
            int[] item2;
            int[][] list;
            bool isUnique;
            Func<int[], bool> filter = null;

            for (int a = 0; a <= 1; a++)
            {
                for (int b = 0; b <= 1; b++)
                {
                    item1 = new int[] { a };
                    item2 = new int[] { b };
                    list = new int[][] { item1, item2 };

                    isUnique =
                        item1[0] != item2[0];

                    Assert.AreEqual(isUnique, Validate.IsUniqueByValue(list, x => x[0], item1[0], alreadyPresent: true));
                    Assert.AreEqual(isUnique, Validate.IsUniqueByValue(list, x => x[0], item1[0], filter, alreadyPresent: true));
                    Assert.AreEqual(isUnique, Validate.IsUniqueByKey(list, x => x[0], item1, alreadyPresent: true));
                    Assert.AreEqual(isUnique, Validate.IsUniqueByKey(list, x => x[0], item1, filter, alreadyPresent: true));
                    Assert.AreEqual(isUnique, Validate.IsUniqueListByKey(list, x => x[0]));

                    for (int c = 0; c <= 1; c++)
                    {
                        for (int d = 0; d <= 1; d++)
                        {
                            item1 = new int[] { a, b };
                            item2 = new int[] { c, d };
                            list = new int[][] { item1, item2 };

                            isUnique =
                                item1[0] != item2[0] ||
                                item1[1] != item2[1];

                            Assert.AreEqual(isUnique, Validate.IsUniqueByValue(list, x => x[0], item1[0], x => x[1], item1[1], alreadyPresent: true));
                            Assert.AreEqual(isUnique, Validate.IsUniqueByValue(list, x => x[0], item1[0], x => x[1], item1[1], filter, alreadyPresent: true));
                            Assert.AreEqual(isUnique, Validate.IsUniqueByKey(list, x => x[0], x => x[1], item1, alreadyPresent: true));
                            Assert.AreEqual(isUnique, Validate.IsUniqueByKey(list, x => x[0], x => x[1], item1, filter, alreadyPresent: true));
                            Assert.AreEqual(isUnique, Validate.IsUniqueListByKey(list, x => x[0], x => x[1]));

                            for (int e = 0; e <= 1; e++)
                            {
                                for (int f = 0; f <= 1; f++)
                                {
                                    item1 = new int[] { a, b, c };
                                    item2 = new int[] { d, e, f };
                                    list = new int[][] { item1, item2 };

                                    isUnique =
                                        item1[0] != item2[0] ||
                                        item1[1] != item2[1] ||
                                        item1[2] != item2[2];

                                    Assert.AreEqual(isUnique, Validate.IsUniqueByValue(list, x => x[0], item1[0], x => x[1], item1[1], x => x[2], item1[2], alreadyPresent: true));
                                    Assert.AreEqual(isUnique, Validate.IsUniqueByValue(list, x => x[0], item1[0], x => x[1], item1[1], x => x[2], item1[2], filter, alreadyPresent: true));
                                    Assert.AreEqual(isUnique, Validate.IsUniqueByKey(list, x => x[0], x => x[1], x => x[2], item1, alreadyPresent: true));
                                    Assert.AreEqual(isUnique, Validate.IsUniqueByKey(list, x => x[0], x => x[1], x => x[2], item1, filter, alreadyPresent: true));
                                    Assert.AreEqual(isUnique, Validate.IsUniqueListByKey(list, x => x[0], x => x[1], x => x[2]));

                                    for (int g = 0; g <= 1; g++)
                                    {
                                        for (int h = 0; h <= 1; h++)
                                        {
                                            item1 = new int[] { a, b, c, d };
                                            item2 = new int[] { e, f, g, h };
                                            list = new int[][] { item1, item2 };

                                            isUnique =
                                                item1[0] != item2[0] ||
                                                item1[1] != item2[1] ||
                                                item1[2] != item2[2] ||
                                                item1[3] != item2[3];

                                            Assert.AreEqual(isUnique, Validate.IsUniqueByValue(list, x => x[0], item1[0], x => x[1], item1[1], x => x[2], item1[2], x => x[3], item1[3], alreadyPresent: true));
                                            Assert.AreEqual(isUnique, Validate.IsUniqueByValue(list, x => x[0], item1[0], x => x[1], item1[1], x => x[2], item1[2], x => x[3], item1[3], filter, alreadyPresent: true));
                                            Assert.AreEqual(isUnique, Validate.IsUniqueByKey(list, x => x[0], x => x[1], x => x[2], x => x[3], item1, alreadyPresent: true));
                                            Assert.AreEqual(isUnique, Validate.IsUniqueByKey(list, x => x[0], x => x[1], x => x[2], x => x[3], item1, filter, alreadyPresent: true));
                                            Assert.AreEqual(isUnique, Validate.IsUniqueListByKey(list, x => x[0], x => x[1], x => x[2], x => x[3]));

                                            for (int i = 0; i <= 1; i++)
                                            {
                                                for (int j = 0; j <= 1; j++)
                                                {
                                                    item1 = new int[] { a, b, c, d, e };
                                                    item2 = new int[] { f, g, h, i, j };
                                                    list = new int[][] { item1, item2 };

                                                    isUnique =
                                                        item1[0] != item2[0] ||
                                                        item1[1] != item2[1] ||
                                                        item1[2] != item2[2] ||
                                                        item1[3] != item2[3] ||
                                                        item1[4] != item2[4];

                                                    Assert.AreEqual(isUnique, Validate.IsUniqueByValue(list, x => x[0], item1[0], x => x[1], item1[1], x => x[2], item1[2], x => x[3], item1[3], x => x[4], item1[4], alreadyPresent: true));
                                                    Assert.AreEqual(isUnique, Validate.IsUniqueByValue(list, x => x[0], item1[0], x => x[1], item1[1], x => x[2], item1[2], x => x[3], item1[3], x => x[4], item1[4], filter, alreadyPresent: true));
                                                    Assert.AreEqual(isUnique, Validate.IsUniqueByKey(list, x => x[0], x => x[1], x => x[2], x => x[3], x => x[4], item1, alreadyPresent: true));
                                                    Assert.AreEqual(isUnique, Validate.IsUniqueByKey(list, x => x[0], x => x[1], x => x[2], x => x[3], x => x[4], item1, filter, alreadyPresent: true));
                                                    Assert.AreEqual(isUnique, Validate.IsUniqueListByKey(list, x => x[0], x => x[1], x => x[2], x => x[3], x => x[4]));

                                                    for (int k = 0; k <= 1; k++)
                                                    {
                                                        for (int l = 0; l <= 1; l++)
                                                        {
                                                            item1 = new int[] { a, b, c, d, e, f };
                                                            item2 = new int[] { g, h, i, j, k, l };
                                                            list = new int[][] { item1, item2 };

                                                            isUnique =
                                                                item1[0] != item2[0] ||
                                                                item1[1] != item2[1] ||
                                                                item1[2] != item2[2] ||
                                                                item1[3] != item2[3] ||
                                                                item1[4] != item2[4] ||
                                                                item1[5] != item2[5];

                                                            Assert.AreEqual(isUnique, Validate.IsUniqueByValue(list, x => x[0], item1[0], x => x[1], item1[1], x => x[2], item1[2], x => x[3], item1[3], x => x[4], item1[4], x => x[5], item1[5], alreadyPresent: true));
                                                            Assert.AreEqual(isUnique, Validate.IsUniqueByValue(list, x => x[0], item1[0], x => x[1], item1[1], x => x[2], item1[2], x => x[3], item1[3], x => x[4], item1[4], x => x[5], item1[5], filter, alreadyPresent: true));
                                                            Assert.AreEqual(isUnique, Validate.IsUniqueByKey(list, x => x[0], x => x[1], x => x[2], x => x[3], x => x[4], x => x[5], item1, alreadyPresent: true));
                                                            Assert.AreEqual(isUnique, Validate.IsUniqueByKey(list, x => x[0], x => x[1], x => x[2], x => x[3], x => x[4], x => x[5], item1, filter, alreadyPresent: true));
                                                            Assert.AreEqual(isUnique, Validate.IsUniqueListByKey(list, x => x[0], x => x[1], x => x[2], x => x[3], x => x[4], x => x[5]));

                                                            for (int m = 0; m <= 1; m++)
                                                            {
                                                                for (int n = 0; n <= 1; n++)
                                                                {
                                                                    item1 = new int[] { a, b, c, d, e, f, g };
                                                                    item2 = new int[] { h, i, j, k, l, m, n };
                                                                    list = new int[][] { item1, item2 };

                                                                    isUnique =
                                                                        item1[0] != item2[0] ||
                                                                        item1[1] != item2[1] ||
                                                                        item1[2] != item2[2] ||
                                                                        item1[3] != item2[3] ||
                                                                        item1[4] != item2[4] ||
                                                                        item1[5] != item2[5] ||
                                                                        item1[6] != item2[6];

                                                                    Assert.AreEqual(isUnique, Validate.IsUniqueByValue(list, x => x[0], item1[0], x => x[1], item1[1], x => x[2], item1[2], x => x[3], item1[3], x => x[4], item1[4], x => x[5], item1[5], x => x[6], item1[6], alreadyPresent: true));
                                                                    Assert.AreEqual(isUnique, Validate.IsUniqueByValue(list, x => x[0], item1[0], x => x[1], item1[1], x => x[2], item1[2], x => x[3], item1[3], x => x[4], item1[4], x => x[5], item1[5], x => x[6], item1[6], filter, alreadyPresent: true));
                                                                    Assert.AreEqual(isUnique, Validate.IsUniqueByKey(list, x => x[0], x => x[1], x => x[2], x => x[3], x => x[4], x => x[5], x => x[6], item1, alreadyPresent: true));
                                                                    Assert.AreEqual(isUnique, Validate.IsUniqueByKey(list, x => x[0], x => x[1], x => x[2], x => x[3], x => x[4], x => x[5], x => x[6], item1, filter, alreadyPresent: true));
                                                                    Assert.AreEqual(isUnique, Validate.IsUniqueListByKey(list, x => x[0], x => x[1], x => x[2], x => x[3], x => x[4], x => x[5], x => x[6]));

                                                                    for (int o = 0; o <= 1; o++)
                                                                    {
                                                                        for (int p = 0; p <= 1; p++)
                                                                        {
                                                                            item1 = new int[] { a, b, c, d, e, f, g, h };
                                                                            item2 = new int[] { i, j, k, l, m, n, o, p };
                                                                            list = new int[][] { item1, item2 };

                                                                            isUnique =
                                                                                item1[0] != item2[0] ||
                                                                                item1[1] != item2[1] ||
                                                                                item1[2] != item2[2] ||
                                                                                item1[3] != item2[3] ||
                                                                                item1[4] != item2[4] ||
                                                                                item1[5] != item2[5] ||
                                                                                item1[6] != item2[6] ||
                                                                                item1[7] != item2[7];

                                                                            Assert.AreEqual(isUnique, Validate.IsUniqueByValue(list, x => x[0], item1[0], x => x[1], item1[1], x => x[2], item1[2], x => x[3], item1[3], x => x[4], item1[4], x => x[5], item1[5], x => x[6], item1[6], x => x[7], item1[7], alreadyPresent: true));
                                                                            Assert.AreEqual(isUnique, Validate.IsUniqueByValue(list, x => x[0], item1[0], x => x[1], item1[1], x => x[2], item1[2], x => x[3], item1[3], x => x[4], item1[4], x => x[5], item1[5], x => x[6], item1[6], x => x[7], item1[7], filter, alreadyPresent: true));
                                                                            Assert.AreEqual(isUnique, Validate.IsUniqueByKey(list, x => x[0], x => x[1], x => x[2], x => x[3], x => x[4], x => x[5], x => x[6], x => x[7], item1, alreadyPresent: true));
                                                                            Assert.AreEqual(isUnique, Validate.IsUniqueByKey(list, x => x[0], x => x[1], x => x[2], x => x[3], x => x[4], x => x[5], x => x[6], x => x[7], item1, filter, alreadyPresent: true));
                                                                            Assert.AreEqual(isUnique, Validate.IsUniqueListByKey(list, x => x[0], x => x[1], x => x[2], x => x[3], x => x[4], x => x[5], x => x[6], x => x[7]));
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }*/

        /*[TestMethod]
        public void Test_Validate_IsUnique_VariableAmountOfArguments2()
        {
            var itemA_noErrors = new int[] { 01, 02, 03, 04, 05, 06, 07, 08 };
            var itemA_errorAt = new int[8][];
            itemA_errorAt[0] = new int[] { 11, 02, 03, 04, 05, 06, 07, 08 };
            itemA_errorAt[1] = new int[] { 01, 12, 03, 04, 05, 06, 07, 08 };
            itemA_errorAt[2] = new int[] { 01, 02, 13, 04, 05, 06, 07, 08 };
            itemA_errorAt[3] = new int[] { 01, 02, 03, 14, 05, 06, 07, 08 };
            itemA_errorAt[4] = new int[] { 01, 02, 03, 04, 15, 06, 07, 08 };
            itemA_errorAt[5] = new int[] { 01, 02, 03, 04, 05, 16, 07, 08 };
            itemA_errorAt[6] = new int[] { 01, 02, 03, 04, 05, 06, 17, 08 };
            itemA_errorAt[7] = new int[] { 01, 02, 03, 04, 05, 06, 07, 18 };

            var itemB_noErrors = new int[] { 11, 12, 13, 14, 15, 16, 17, 18 };
            var itemB_errorAt = new int[8][];
            itemB_errorAt[0] = new int[] { 01, 12, 13, 14, 15, 16, 17, 18 };
            itemB_errorAt[1] = new int[] { 11, 02, 13, 14, 15, 16, 17, 18 };
            itemB_errorAt[2] = new int[] { 11, 12, 03, 14, 15, 16, 17, 18 };
            itemB_errorAt[3] = new int[] { 11, 12, 13, 04, 15, 16, 17, 18 };
            itemB_errorAt[4] = new int[] { 11, 12, 13, 14, 05, 16, 17, 18 };
            itemB_errorAt[5] = new int[] { 11, 12, 13, 14, 15, 06, 17, 18 };
            itemB_errorAt[6] = new int[] { 11, 12, 13, 14, 15, 16, 07, 18 };
            itemB_errorAt[7] = new int[] { 11, 12, 13, 14, 15, 16, 17, 08 };

            int[][] list;
            bool isUnique;
            Func<int[], bool> filter = null;

            // 1-part key

            list = new int[][] { itemA_noErrors, itemB_noErrors };
            isUnique = true;

            Assert.AreEqual(isUnique, Validate.IsUniqueByValue(list, x => x[0], itemA_noErrors[0], alreadyPresent: true));
            Assert.AreEqual(isUnique, Validate.IsUniqueByValue(list, x => x[0], itemA_noErrors[0], filter, alreadyPresent: true));
            Assert.AreEqual(isUnique, Validate.IsUniqueByKey(list, x => x[0], itemA_noErrors, alreadyPresent: true));
            Assert.AreEqual(isUnique, Validate.IsUniqueByKey(list, x => x[0], itemA_noErrors, filter, alreadyPresent: true));
            Assert.AreEqual(isUnique, Validate.IsUniqueListByKey(list, x => x[0]));

            list = new int[][] { itemA_errorAt[0], itemB_noErrors };
            isUnique = false;

            Assert.AreEqual(isUnique, Validate.IsUniqueByValue(list, x => x[0], itemB_noErrors[0], alreadyPresent: true));
            Assert.AreEqual(isUnique, Validate.IsUniqueByValue(list, x => x[0], itemB_noErrors[0], filter, alreadyPresent: true));
            Assert.AreEqual(isUnique, Validate.IsUniqueByKey(list, x => x[0], itemB_noErrors, alreadyPresent: true));
            Assert.AreEqual(isUnique, Validate.IsUniqueByKey(list, x => x[0], itemB_noErrors, filter, alreadyPresent: true));
            Assert.AreEqual(isUnique, Validate.IsUniqueListByKey(list, x => x[0]));

            list = new int[][] { itemA_noErrors, itemB_errorAt[0] };
            isUnique = false;

            Assert.AreEqual(isUnique, Validate.IsUniqueByValue(list, x => x[0], itemA_noErrors[0], alreadyPresent: true));
            Assert.AreEqual(isUnique, Validate.IsUniqueByValue(list, x => x[0], itemA_noErrors[0], filter, alreadyPresent: true));
            Assert.AreEqual(isUnique, Validate.IsUniqueByKey(list, x => x[0], itemA_noErrors, alreadyPresent: true));
            Assert.AreEqual(isUnique, Validate.IsUniqueByKey(list, x => x[0], itemA_noErrors, filter, alreadyPresent: true));
            Assert.AreEqual(isUnique, Validate.IsUniqueListByKey(list, x => x[0]));

            // 2-part key

            list = new int[][] { itemA_noErrors, itemB_noErrors };
            isUnique = true;

            Assert.AreEqual(isUnique, Validate.IsUniqueByValue(list, x => x[0], itemA_noErrors[0], x => x[1], itemA_noErrors[1], alreadyPresent: true));
            Assert.AreEqual(isUnique, Validate.IsUniqueByValue(list, x => x[0], itemA_noErrors[0], x => x[1], itemA_noErrors[1], filter, alreadyPresent: true));
            Assert.AreEqual(isUnique, Validate.IsUniqueByKey(list, x => x[0], x => x[1], itemA_noErrors, alreadyPresent: true));
            Assert.AreEqual(isUnique, Validate.IsUniqueByKey(list, x => x[0], x => x[1], itemA_noErrors, filter, alreadyPresent: true));
            Assert.AreEqual(isUnique, Validate.IsUniqueListByKey(list, x => x[0], x => x[1]));

            list = new int[][] { itemA_errorAt[0], itemB_noErrors };
            isUnique = false;

            Assert.AreEqual(isUnique, Validate.IsUniqueByValue(list, x => x[0], itemB_noErrors[0], x => x[1], itemB_noErrors[1], alreadyPresent: true));
            Assert.AreEqual(isUnique, Validate.IsUniqueByValue(list, x => x[0], itemB_noErrors[0], x => x[1], itemB_noErrors[1], filter, alreadyPresent: true));
            Assert.AreEqual(isUnique, Validate.IsUniqueByKey(list, x => x[0], x => x[1], itemB_noErrors, alreadyPresent: true));
            Assert.AreEqual(isUnique, Validate.IsUniqueByKey(list, x => x[0], x => x[1], itemB_noErrors, filter, alreadyPresent: true));
            Assert.AreEqual(isUnique, Validate.IsUniqueListByKey(list, x => x[0], x => x[1]));

            list = new int[][] { itemA_errorAt[1], itemB_noErrors };
            isUnique = false;

            Assert.AreEqual(isUnique, Validate.IsUniqueByValue(list, x => x[0], itemB_noErrors[0], x => x[1], itemB_noErrors[1], alreadyPresent: true));
            Assert.AreEqual(isUnique, Validate.IsUniqueByValue(list, x => x[0], itemB_noErrors[0], x => x[1], itemB_noErrors[1], filter, alreadyPresent: true));
            Assert.AreEqual(isUnique, Validate.IsUniqueByKey(list, x => x[0], x => x[1], itemB_noErrors, alreadyPresent: true));
            Assert.AreEqual(isUnique, Validate.IsUniqueByKey(list, x => x[0], x => x[1], itemB_noErrors, filter, alreadyPresent: true));
            Assert.AreEqual(isUnique, Validate.IsUniqueListByKey(list, x => x[0], x => x[1]));

            list = new int[][] { itemA_noErrors, itemB_errorAt[0] };
            isUnique = false;

            Assert.AreEqual(isUnique, Validate.IsUniqueByValue(list, x => x[0], itemA_noErrors[0], x => x[1], itemA_noErrors[1], alreadyPresent: true));
            Assert.AreEqual(isUnique, Validate.IsUniqueByValue(list, x => x[0], itemA_noErrors[0], x => x[1], itemA_noErrors[1], filter, alreadyPresent: true));
            Assert.AreEqual(isUnique, Validate.IsUniqueByKey(list, x => x[0], x => x[1], itemA_noErrors, alreadyPresent: true));
            Assert.AreEqual(isUnique, Validate.IsUniqueByKey(list, x => x[0], x => x[1], itemA_noErrors, filter, alreadyPresent: true));
            Assert.AreEqual(isUnique, Validate.IsUniqueListByKey(list, x => x[0], x => x[1]));

            list = new int[][] { itemA_noErrors, itemB_errorAt[1] };
            isUnique = false;

            Assert.AreEqual(isUnique, Validate.IsUniqueByValue(list, x => x[0], itemA_noErrors[0], x => x[1], itemA_noErrors[1], alreadyPresent: true));
            Assert.AreEqual(isUnique, Validate.IsUniqueByValue(list, x => x[0], itemA_noErrors[0], x => x[1], itemA_noErrors[1], filter, alreadyPresent: true));
            Assert.AreEqual(isUnique, Validate.IsUniqueByKey(list, x => x[0], x => x[1], itemA_noErrors, alreadyPresent: true));
            Assert.AreEqual(isUnique, Validate.IsUniqueByKey(list, x => x[0], x => x[1], itemA_noErrors, filter, alreadyPresent: true));
            Assert.AreEqual(isUnique, Validate.IsUniqueListByKey(list, x => x[0], x => x[1]));
        }

        [TestMethod]
        public void Test_Validate_IsUnique_VariableAmountOfArguments3()
        {
            var itemA_noErrors = new int[] { 01, 02, 03, 04, 05, 06, 07, 08 };
            var itemA_errorAt = new int[8][];
            itemA_errorAt[0] = new int[] { 11, 02, 03, 04, 05, 06, 07, 08 };
            itemA_errorAt[1] = new int[] { 01, 12, 03, 04, 05, 06, 07, 08 };
            itemA_errorAt[2] = new int[] { 01, 02, 13, 04, 05, 06, 07, 08 };
            itemA_errorAt[3] = new int[] { 01, 02, 03, 14, 05, 06, 07, 08 };
            itemA_errorAt[4] = new int[] { 01, 02, 03, 04, 15, 06, 07, 08 };
            itemA_errorAt[5] = new int[] { 01, 02, 03, 04, 05, 16, 07, 08 };
            itemA_errorAt[6] = new int[] { 01, 02, 03, 04, 05, 06, 17, 08 };
            itemA_errorAt[7] = new int[] { 01, 02, 03, 04, 05, 06, 07, 18 };

            var itemB_noErrors = new int[] { 11, 12, 13, 14, 15, 16, 17, 18 };
            var itemB_errorAt = new int[8][];
            itemB_errorAt[0] = new int[] { 01, 12, 13, 14, 15, 16, 17, 18 };
            itemB_errorAt[1] = new int[] { 11, 02, 13, 14, 15, 16, 17, 18 };
            itemB_errorAt[2] = new int[] { 11, 12, 03, 14, 15, 16, 17, 18 };
            itemB_errorAt[3] = new int[] { 11, 12, 13, 04, 15, 16, 17, 18 };
            itemB_errorAt[4] = new int[] { 11, 12, 13, 14, 05, 16, 17, 18 };
            itemB_errorAt[5] = new int[] { 11, 12, 13, 14, 15, 06, 17, 18 };
            itemB_errorAt[6] = new int[] { 11, 12, 13, 14, 15, 16, 07, 18 };
            itemB_errorAt[7] = new int[] { 11, 12, 13, 14, 15, 16, 17, 08 };

            int[][] list;
            bool isUnique;
            int[] item;
            int keyPartCount;
            Func<int[], bool> filter = null;

            // 1-part key

            Action<int[][], int[], bool> test1PartKey = (l, i, isUn) =>
            {
                Assert.AreEqual(isUn, Validate.IsUniqueByValue(l, x => x[0], i[0], alreadyPresent: true));
                Assert.AreEqual(isUn, Validate.IsUniqueByValue(l, x => x[0], i[0], filter, alreadyPresent: true));
                Assert.AreEqual(isUn, Validate.IsUniqueByKey(l, x => x[0], i, alreadyPresent: true));
                Assert.AreEqual(isUn, Validate.IsUniqueByKey(l, x => x[0], i, filter, alreadyPresent: true));
                Assert.AreEqual(isUn, Validate.IsUniqueListByKey(l, x => x[0]));
            };

            keyPartCount = 1;

            list = new int[][] { itemA_noErrors, itemB_noErrors };
            item = itemA_noErrors;
            isUnique = true;

            test1PartKey(list, item, isUnique);

            for (int i = 0; i < keyPartCount; i++)
            {
                list = new int[][] { itemA_errorAt[i], itemB_noErrors };
                item = itemB_noErrors;
                isUnique = false;

                test1PartKey(list, item, isUnique);
            }

            for (int i = 0; i < keyPartCount; i++)
            {
                list = new int[][] { itemA_noErrors, itemB_errorAt[i] };
                item = itemA_noErrors;
                isUnique = false;

                test1PartKey(list, item, isUnique);
            }

            // 2-part key

            Action<int[][], int[], bool> test2PartKey = (l, i, isUn) =>
            {
                Assert.AreEqual(isUn, Validate.IsUniqueByValue(l, x => x[0], i[0], x => x[1], i[1], alreadyPresent: true));
                Assert.AreEqual(isUn, Validate.IsUniqueByValue(l, x => x[0], i[0], x => x[1], i[1], filter, alreadyPresent: true));
                Assert.AreEqual(isUn, Validate.IsUniqueByKey(l, x => x[0], x => x[1], i, alreadyPresent: true));
                Assert.AreEqual(isUn, Validate.IsUniqueByKey(l, x => x[0], x => x[1], i, filter, alreadyPresent: true));
                Assert.AreEqual(isUn, Validate.IsUniqueListByKey(l, x => x[0], x => x[1]));
            };

            keyPartCount = 2;

            list = new int[][] { itemA_noErrors, itemB_noErrors };
            item = itemA_noErrors;
            isUnique = true;

            test2PartKey(list, item, isUnique);

            for (int i = 0; i < keyPartCount; i++)
            {
                list = new int[][] { itemA_errorAt[i], itemB_noErrors };
                item = itemB_noErrors;
                isUnique = false;

                test2PartKey(list, item, isUnique);
            }

            for (int i = 0; i < keyPartCount; i++)
            {
                list = new int[][] { itemA_noErrors, itemB_errorAt[i] };
                item = itemA_noErrors;
                isUnique = false;

                test2PartKey(list, item, isUnique);
            }

            // 3-part key

            Action<int[][], int[], bool> test3PartKey = (l, i, isUn) =>
            {
                Assert.AreEqual(isUn, Validate.IsUniqueByValue(l, x => x[0], i[0], x => x[1], i[1], x => x[2], i[2], alreadyPresent: true));
                Assert.AreEqual(isUn, Validate.IsUniqueByValue(l, x => x[0], i[0], x => x[1], i[1], x => x[2], i[2], filter, alreadyPresent: true));
                Assert.AreEqual(isUn, Validate.IsUniqueByKey(l, x => x[0], x => x[1], x => x[2], i, alreadyPresent: true));
                Assert.AreEqual(isUn, Validate.IsUniqueByKey(l, x => x[0], x => x[1], x => x[2], i, filter, alreadyPresent: true));
                Assert.AreEqual(isUn, Validate.IsUniqueListByKey(l, x => x[0], x => x[1], x => x[2]));
            };

            keyPartCount = 3;

            list = new int[][] { itemA_noErrors, itemB_noErrors };
            item = itemA_noErrors;
            isUnique = true;

            test2PartKey(list, item, isUnique);

            for (int i = 0; i < keyPartCount; i++)
            {
                list = new int[][] { itemA_errorAt[i], itemB_noErrors };
                item = itemB_noErrors;
                isUnique = false;

                test2PartKey(list, item, isUnique);
            }

            for (int i = 0; i < keyPartCount; i++)
            {
                list = new int[][] { itemA_noErrors, itemB_errorAt[i] };
                item = itemA_noErrors;
                isUnique = false;

                test2PartKey(list, item, isUnique);
            }
        }

        [TestMethod]
        public void Test_Validate_IsUnique_VariableAmountOfArguments4()
        {
            var itemA_noErrors = new int[] { 01, 02, 03, 04, 05, 06, 07, 08 };
            var itemA_errorAt = new int[8][];
            itemA_errorAt[0] = new int[] { 11, 02, 03, 04, 05, 06, 07, 08 };
            itemA_errorAt[1] = new int[] { 01, 12, 03, 04, 05, 06, 07, 08 };
            itemA_errorAt[2] = new int[] { 01, 02, 13, 04, 05, 06, 07, 08 };
            itemA_errorAt[3] = new int[] { 01, 02, 03, 14, 05, 06, 07, 08 };
            itemA_errorAt[4] = new int[] { 01, 02, 03, 04, 15, 06, 07, 08 };
            itemA_errorAt[5] = new int[] { 01, 02, 03, 04, 05, 16, 07, 08 };
            itemA_errorAt[6] = new int[] { 01, 02, 03, 04, 05, 06, 17, 08 };
            itemA_errorAt[7] = new int[] { 01, 02, 03, 04, 05, 06, 07, 18 };

            var itemB_noErrors = new int[] { 11, 12, 13, 14, 15, 16, 17, 18 };
            var itemB_errorAt = new int[8][];
            itemB_errorAt[0] = new int[] { 01, 12, 13, 14, 15, 16, 17, 18 };
            itemB_errorAt[1] = new int[] { 11, 02, 13, 14, 15, 16, 17, 18 };
            itemB_errorAt[2] = new int[] { 11, 12, 03, 14, 15, 16, 17, 18 };
            itemB_errorAt[3] = new int[] { 11, 12, 13, 04, 15, 16, 17, 18 };
            itemB_errorAt[4] = new int[] { 11, 12, 13, 14, 05, 16, 17, 18 };
            itemB_errorAt[5] = new int[] { 11, 12, 13, 14, 15, 06, 17, 18 };
            itemB_errorAt[6] = new int[] { 11, 12, 13, 14, 15, 16, 07, 18 };
            itemB_errorAt[7] = new int[] { 11, 12, 13, 14, 15, 16, 17, 08 };

            int[][] list;
            bool isUnique;
            int[] item;
            Func<int[], bool> filter = null;
            Action<int[][], int[], bool> xPartKeyTest;

            Action<int, Action<int[][], int[], bool>> runXPartKeyTests = (keyPartCount, xPartKeyTest2) =>
            {
                list = new int[][] { itemA_noErrors, itemB_noErrors };
                item = itemA_noErrors;
                isUnique = true;

                xPartKeyTest2(list, item, isUnique);

                for (int i = 0; i < keyPartCount; i++)
                {
                    list = new int[][] { itemA_errorAt[i], itemB_noErrors };
                    item = itemB_noErrors;
                    isUnique = false;

                    xPartKeyTest2(list, item, isUnique);
                }

                for (int i = 0; i < keyPartCount; i++)
                {
                    list = new int[][] { itemA_noErrors, itemB_errorAt[i] };
                    item = itemA_noErrors;
                    isUnique = false;

                    xPartKeyTest2(list, item, isUnique);
                }
            };

            // 1-part key

            xPartKeyTest = (l, i, isUn) =>
            {
                Assert.AreEqual(isUn, Validate.IsUniqueByValue(l, x => x[0], i[0], alreadyPresent: true));
                Assert.AreEqual(isUn, Validate.IsUniqueByValue(l, x => x[0], i[0], filter, alreadyPresent: true));
                Assert.AreEqual(isUn, Validate.IsUniqueByKey(l, x => x[0], i, alreadyPresent: true));
                Assert.AreEqual(isUn, Validate.IsUniqueByKey(l, x => x[0], i, filter, alreadyPresent: true));
                Assert.AreEqual(isUn, Validate.IsUniqueListByKey(l, x => x[0]));
            };

            runXPartKeyTests(1, xPartKeyTest);

            // 2-part key

            xPartKeyTest = (l, i, isUn) =>
            {
                Assert.AreEqual(isUn, Validate.IsUniqueByValue(l, x => x[0], i[0], x => x[1], i[1], alreadyPresent: true));
                Assert.AreEqual(isUn, Validate.IsUniqueByValue(l, x => x[0], i[0], x => x[1], i[1], filter, alreadyPresent: true));
                Assert.AreEqual(isUn, Validate.IsUniqueByKey(l, x => x[0], x => x[1], i, alreadyPresent: true));
                Assert.AreEqual(isUn, Validate.IsUniqueByKey(l, x => x[0], x => x[1], i, filter, alreadyPresent: true));
                Assert.AreEqual(isUn, Validate.IsUniqueListByKey(l, x => x[0], x => x[1]));
            };

            runXPartKeyTests(2, xPartKeyTest);

            // 3-part key

            xPartKeyTest = (l, i, isUn) =>
            {
                Assert.AreEqual(isUn, Validate.IsUniqueByValue(l, x => x[0], i[0], x => x[1], i[1], x => x[2], i[2], alreadyPresent: true));
                Assert.AreEqual(isUn, Validate.IsUniqueByValue(l, x => x[0], i[0], x => x[1], i[1], x => x[2], i[2], filter, alreadyPresent: true));
                Assert.AreEqual(isUn, Validate.IsUniqueByKey(l, x => x[0], x => x[1], x => x[2], i, alreadyPresent: true));
                Assert.AreEqual(isUn, Validate.IsUniqueByKey(l, x => x[0], x => x[1], x => x[2], i, filter, alreadyPresent: true));
                Assert.AreEqual(isUn, Validate.IsUniqueListByKey(l, x => x[0], x => x[1], x => x[2]));
            };

            runXPartKeyTests(3, xPartKeyTest);

            // 4-part key

            xPartKeyTest = (l, i, isUn) =>
            {
                Assert.AreEqual(isUn, Validate.IsUniqueByValue(l, x => x[0], i[0], x => x[1], i[1], x => x[2], i[2], x => x[3], i[3], alreadyPresent: true));
                Assert.AreEqual(isUn, Validate.IsUniqueByValue(l, x => x[0], i[0], x => x[1], i[1], x => x[2], i[2], x => x[3], i[3], filter, alreadyPresent: true));
                Assert.AreEqual(isUn, Validate.IsUniqueByKey(l, x => x[0], x => x[1], x => x[2], x => x[3], i, alreadyPresent: true));
                Assert.AreEqual(isUn, Validate.IsUniqueByKey(l, x => x[0], x => x[1], x => x[2], x => x[3], i, filter, alreadyPresent: true));
                Assert.AreEqual(isUn, Validate.IsUniqueListByKey(l, x => x[0], x => x[1], x => x[2], x => x[3]));
            };

            runXPartKeyTests(4, xPartKeyTest);

            // 5-part key

            xPartKeyTest = (l, i, isUn) =>
            {
                Assert.AreEqual(isUn, Validate.IsUniqueByValue(l, x => x[0], i[0], x => x[1], i[1], x => x[2], i[2], x => x[3], i[3], x => x[4], i[4], alreadyPresent: true));
                Assert.AreEqual(isUn, Validate.IsUniqueByValue(l, x => x[0], i[0], x => x[1], i[1], x => x[2], i[2], x => x[3], i[3], x => x[4], i[4], filter, alreadyPresent: true));
                Assert.AreEqual(isUn, Validate.IsUniqueByKey(l, x => x[0], x => x[1], x => x[2], x => x[3], x => x[4], i, alreadyPresent: true));
                Assert.AreEqual(isUn, Validate.IsUniqueByKey(l, x => x[0], x => x[1], x => x[2], x => x[3], x => x[4], i, filter, alreadyPresent: true));
                Assert.AreEqual(isUn, Validate.IsUniqueListByKey(l, x => x[0], x => x[1], x => x[2], x => x[3], x => x[4]));
            };

            runXPartKeyTests(5, xPartKeyTest);

            // 6-part key

            xPartKeyTest = (l, i, isUn) =>
            {
                Assert.AreEqual(isUn, Validate.IsUniqueByValue(l, x => x[0], i[0], x => x[1], i[1], x => x[2], i[2], x => x[3], i[3], x => x[4], i[4], x => x[5], i[5], alreadyPresent: true));
                Assert.AreEqual(isUn, Validate.IsUniqueByValue(l, x => x[0], i[0], x => x[1], i[1], x => x[2], i[2], x => x[3], i[3], x => x[4], i[4], x => x[5], i[5], filter, alreadyPresent: true));
                Assert.AreEqual(isUn, Validate.IsUniqueByKey(l, x => x[0], x => x[1], x => x[2], x => x[3], x => x[4], x => x[5], i, alreadyPresent: true));
                Assert.AreEqual(isUn, Validate.IsUniqueByKey(l, x => x[0], x => x[1], x => x[2], x => x[3], x => x[4], x => x[5], i, filter, alreadyPresent: true));
                Assert.AreEqual(isUn, Validate.IsUniqueListByKey(l, x => x[0], x => x[1], x => x[2], x => x[3], x => x[4], x => x[5]));
            };

            runXPartKeyTests(6, xPartKeyTest);

            // 7-part key

            xPartKeyTest = (l, i, isUn) =>
            {
                Assert.AreEqual(isUn, Validate.IsUniqueByValue(l, x => x[0], i[0], x => x[1], i[1], x => x[2], i[2], x => x[3], i[3], x => x[4], i[4], x => x[5], i[5], x => x[6], i[6], alreadyPresent: true));
                Assert.AreEqual(isUn, Validate.IsUniqueByValue(l, x => x[0], i[0], x => x[1], i[1], x => x[2], i[2], x => x[3], i[3], x => x[4], i[4], x => x[5], i[5], x => x[6], i[6], filter, alreadyPresent: true));
                Assert.AreEqual(isUn, Validate.IsUniqueByKey(l, x => x[0], x => x[1], x => x[2], x => x[3], x => x[4], x => x[5], x => x[6], i, alreadyPresent: true));
                Assert.AreEqual(isUn, Validate.IsUniqueByKey(l, x => x[0], x => x[1], x => x[2], x => x[3], x => x[4], x => x[5], x => x[6], i, filter, alreadyPresent: true));
                Assert.AreEqual(isUn, Validate.IsUniqueListByKey(l, x => x[0], x => x[1], x => x[2], x => x[3], x => x[4], x => x[5], x => x[6]));
            };

            runXPartKeyTests(7, xPartKeyTest);

            // 8-part key

            xPartKeyTest = (l, i, isUn) =>
            {
                Assert.AreEqual(isUn, Validate.IsUniqueByValue(l, x => x[0], i[0], x => x[1], i[1], x => x[2], i[2], x => x[3], i[3], x => x[4], i[4], x => x[5], i[5], x => x[6], i[6], x => x[7], i[7], alreadyPresent: true));
                Assert.AreEqual(isUn, Validate.IsUniqueByValue(l, x => x[0], i[0], x => x[1], i[1], x => x[2], i[2], x => x[3], i[3], x => x[4], i[4], x => x[5], i[5], x => x[6], i[6], x => x[7], i[7], filter, alreadyPresent: true));
                Assert.AreEqual(isUn, Validate.IsUniqueByKey(l, x => x[0], x => x[1], x => x[2], x => x[3], x => x[4], x => x[5], x => x[6], x => x[7], i, alreadyPresent: true));
                Assert.AreEqual(isUn, Validate.IsUniqueByKey(l, x => x[0], x => x[1], x => x[2], x => x[3], x => x[4], x => x[5], x => x[6], x => x[7], i, filter, alreadyPresent: true));
                Assert.AreEqual(isUn, Validate.IsUniqueListByKey(l, x => x[0], x => x[1], x => x[2], x => x[3], x => x[4], x => x[5], x => x[6], x => x[7]));
            };

            runXPartKeyTests(8, xPartKeyTest);
        }*/
